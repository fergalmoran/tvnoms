using System.Net;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Settings.Configuration;
using server.ServiceDefaults;
using TvNoms.Core.Entities;
using TvNoms.Core.Models;
using TvNoms.Core.Utilities;
using TvNoms.Infrastructure.Identity;
using TvNoms.Infrastructure.Jobs;
using TvNoms.Infrastructure.MediaLookup;
using TvNoms.Infrastructure.Messaging.Email;
using TvNoms.Infrastructure.Messaging.SMS;
using TvNoms.Infrastructure.Storage;
using TvNoms.Infrastructure.ViewRenderer.Razor;
using TvNoms.Server.ApiService.Shared;
using TvNoms.Server.Data;
using TvNoms.Server.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);
var assemblies = AssemblyHelper.GetAssemblies().ToArray();

Log.Logger = new LoggerConfiguration()
  .ReadFrom
  .Configuration(
    builder.Configuration,
    new ConfigurationReaderOptions { SectionName = "SerilogOptions" })
  .Enrich
  .FromLogContext()
  .CreateLogger();

builder.Logging.ClearProviders();
builder.Host.UseSerilog(Log.Logger);


// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// if (builder.Environment.IsDevelopment()) {
//   builder.WebHost.ConfigureKestrel(serverOptions => {
//     var pemFile = builder.Configuration["Startup:PemFile"];
//     var keyFile = builder.Configuration["Startup:KeyFile"];
//     var port = int.Parse(builder.Configuration["Startup:Port"] ?? "5001");
//     if (string.IsNullOrEmpty(pemFile) || string.IsNullOrEmpty(keyFile)) {
//       throw new InvalidOperationException("Unable to find SSL certificate files.");
//     }
//
//     serverOptions.Listen(IPAddress.Any, port, listenOptions => {
//       var certPem = File.ReadAllText(pemFile);
//       var keyPem = File.ReadAllText(keyFile);
//       var x509 = X509Certificate2.CreateFromPem(certPem, keyPem);
//
//       listenOptions.UseHttps(x509);
//     });
//   });
// }

builder.Services.AddDbContext<AppDbContext>(options => {
  var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
  options
    .UseNpgsql(connectionString,
      sqlOptions => sqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.GetName().Name))
    .UseSnakeCaseNamingConvention();
});
builder.Services.AddAutoMapper(assemblies);
builder.Services.AddMediatR(options => { options.RegisterServicesFromAssemblies(assemblies); });
builder.Services.AddRepositories(assemblies);
builder.Services.AddValidators(assemblies);
builder.Services.AddMediaLookupServices(builder.Configuration);
builder.Services.AddScheduledJobs(builder.Configuration);

builder.Services.AddIdentity<User, Role>(options => {
    // Password settings. (Will be using fluent validation)
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 0;
    options.Password.RequiredUniqueChars = 0;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters = string.Empty;
    options.User.RequireUniqueEmail = false;

    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;

    // Generate Short Code for Email Confirmation using Asp.Net Identity core 2.1
    // source: https://stackoverflow.com/questions/53616142/generate-short-code-for-email-confirmation-using-asp-net-identity-core-2-1
    options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
    options.Tokens.ChangeEmailTokenProvider = TokenOptions.DefaultEmailProvider;
    options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;

    options.ClaimsIdentity.RoleClaimType = ClaimTypes.Role;
    options.ClaimsIdentity.UserNameClaimType = ClaimTypes.Name;
    options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier;
    options.ClaimsIdentity.EmailClaimType = ClaimTypes.Email;
    options.ClaimsIdentity.SecurityStampClaimType = ClaimTypes.SerialNumber;
  })
  .AddEntityFrameworkStores<AppDbContext>()
  .AddDefaultTokenProviders();
builder.Services.AddClientContext();
builder.Services.AddHttpContextAccessor();

builder.Services.AddWebAppCors(builder.Configuration);
builder.Services.AddAuthentication(options => {
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  })
  .AddBearer(options => {
    builder.Configuration.GetRequiredSection("BearerAuthOptions").Bind(options);
  })
  .AddGoogle(GoogleDefaults.AuthenticationScheme, options => {
    options.SignInScheme = IdentityConstants.ExternalScheme;
    builder.Configuration.GetRequiredSection("GoogleAuthOptions").Bind(options);
  });
builder.Services.AddAuthorization();

builder.Services.AddModelBuilder();
builder.Services.AddMailgunEmailSender(options => {
  builder.Configuration.GetRequiredSection("MailgunEmailOptions").Bind(options);
});
builder.Services.AddFakeSmsSender();
builder.Services.AddRazorViewRenderer();

builder.Services.AddLocalFileStorage(options => {
  options.RootPath = Path.Combine(builder.Environment.WebRootPath, "uploads");
  options.WebRootPath = "/uploads";
});
builder.Services.AddDocumentations();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
  options.IdleTimeout = TimeSpan.FromSeconds(10);
  options.Cookie.HttpOnly = true;
  options.Cookie.IsEssential = true;
});
builder.Services.ConfigureHttpJsonOptions(options => {
  options.SerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
  options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

  options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
  options.SerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

  options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();
app.UseCors("WebAppCors");

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment()) {
  app.UseSwagger();
  app.UseSwaggerUI();
}


app.MapGet("/ping", () => "pong");


app.MapDefaultEndpoints();
app.MapEndpoints();

app.Run();
