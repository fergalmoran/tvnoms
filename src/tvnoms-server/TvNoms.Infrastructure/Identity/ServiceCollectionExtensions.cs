using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TvNoms.Core.Entities;
using TvNoms.Core.Extensions.Identity;
using TvNoms.Server.Services;

namespace TvNoms.Infrastructure.Identity;

public static class ServiceCollectionExtensions {
  public static IServiceCollection AddWebAppCors(this IServiceCollection services, IConfiguration config) {
    services.AddCors(options => {
      options.AddPolicy("WebAppCors", policy => {
        // options.AddDefaultPolicy(policy => {
        var allowedOrigins =
          config.GetSection("AllowedOrigins")?.Get<string[]>() ?? Array.Empty<string>();

        policy
          .WithOrigins(allowedOrigins)
          .AllowAnyMethod()
          .AllowAnyHeader()
          .AllowCredentials()
          // .WithExposedHeaders("Content-Disposition")
          // .SetPreflightMaxAge(TimeSpan.FromMinutes(10))
          ;
      });
    });
    return services;
  }

  public static AuthenticationBuilder AddBearer(this AuthenticationBuilder builder,
    Action<UserSessionOptions> options) {
    builder.Services.AddOptions<UserSessionOptions>().Configure<IHttpContextAccessor>(
      (optionsInstance, httpContextAccessor) => {
        ConfigureBearer(() => options(optionsInstance), optionsInstance, httpContextAccessor);
      });
    builder.AddBearer();
    return builder;
  }

  private static void ConfigureBearer(Action configure, UserSessionOptions options,
    IHttpContextAccessor httpContextAccessor) {
    configure();

    var context = httpContextAccessor?.HttpContext;
    var serverUrl = context != null
      ? string.Concat(context.Request.Scheme, "://", context.Request.Host.ToUriComponent())
      : string.Empty;

    var separator = UserSessionOptions.ValueSeparator;

    options.Secret = !string.IsNullOrEmpty(options.Secret) ? options.Secret : TokenHelper.Secret;
    options.Issuer = string
      .Join(separator, (options.Issuer)
        .Split(separator)
        .Append(serverUrl)
        .Distinct()
        .SkipWhile(string.IsNullOrEmpty)
        .ToArray());
    options.Audience = string
      .Join(separator, (options.Audience)
        .Split(separator)
        .Append(serverUrl)
        .Distinct()
        .SkipWhile(string.IsNullOrEmpty)
        .ToArray());

    options.AccessTokenExpiresIn = options.AccessTokenExpiresIn != TimeSpan.Zero
      ? options.AccessTokenExpiresIn
      : TimeSpan.FromDays(1);
    options.RefreshTokenExpiresIn = options.RefreshTokenExpiresIn != TimeSpan.Zero
      ? options.RefreshTokenExpiresIn
      : TimeSpan.FromDays(90);
  }

  public static AuthenticationBuilder AddBearer(this AuthenticationBuilder builder) {
    builder.Services.AddScoped<IUserClaimsPrincipalFactory<User>, UserClaimsPrincipalFactory>();
    builder.Services.AddScoped<IUserSessionFactory, UserSessionFactory>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.ConfigureOptions<ConfigureJwtBearerOptions>();
    return builder.AddJwtBearer();
  }

  public static IServiceCollection AddClientContext(this IServiceCollection services) {
    return services.AddScoped<IClientContext, ClientContext>();
  }
}
