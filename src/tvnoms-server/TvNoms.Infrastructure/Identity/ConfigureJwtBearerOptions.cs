using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using TvNoms.Core.Extensions.Identity;
using TvNoms.Core.Repositories;
using TvNoms.Server.Data.Repositories;

namespace TvNoms.Infrastructure.Identity;

public class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions> {
  private readonly IOptions<UserSessionOptions> _userSessionOptions;

  public ConfigureJwtBearerOptions(IOptions<UserSessionOptions> userSessionOptions) {
    _userSessionOptions = userSessionOptions ?? throw new ArgumentNullException(nameof(userSessionOptions));
  }

  public void Configure(JwtBearerOptions options) {
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters ??= new TokenValidationParameters();
    options.TokenValidationParameters.ValidateIssuer = true;
    options.TokenValidationParameters.ValidIssuers = _userSessionOptions.Value.GetIssuers();
    options.TokenValidationParameters.ValidateAudience = true;
    options.TokenValidationParameters.ValidAudiences = _userSessionOptions.Value.GetAudiences();
    options.TokenValidationParameters.ValidateLifetime = true;
    options.TokenValidationParameters.ValidateIssuerSigningKey = true;
    options.TokenValidationParameters.IssuerSigningKey =
      new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_userSessionOptions.Value.Secret));

    options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;

    options.Events = new JwtBearerEvents {
      OnAuthenticationFailed = context => {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>()
          .CreateLogger(nameof(JwtBearerEvents));
        logger.LogError(context.Exception, $"JWT Authentication failed");
        return Task.CompletedTask;
      },
      OnTokenValidated = async context => {
        var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
        var userSessionFactory = context.HttpContext.RequestServices.GetRequiredService<IUserSessionFactory>();
        var clientContext = context.HttpContext.RequestServices.GetRequiredService<IClientContext>();

        var claimsPrincipal = context.Principal;

        if (claimsPrincipal?.Claims == null || !claimsPrincipal.Claims.Any()) {
          context.Fail("This is not our issued token. It has no claims.");
          return;
        }

        //Allow the bearer token to be used in Postman if we're debugging
        if (false) {
          var deviceId = userRepository.GetDeviceId(claimsPrincipal);
          if (deviceId == null ||
              !string.Equals(
                deviceId,
                clientContext.DeviceId,
                StringComparison.Ordinal)) {
            context.Fail("Detected usage of an old token from an unknown device! Please login again!");
            return;
          }
        }

        var userId = userRepository.GetUserId(claimsPrincipal);
        var user = userId.HasValue ? await userRepository.GetByIdAsync(userId.Value) : null;

        var securityStamp = userRepository.GetSecurityStamp(claimsPrincipal);

        if (user == null || !string.Equals(user.SecurityStamp, securityStamp, StringComparison.Ordinal) ||
            !user.Active) {
          // user has changed his/her password/roles/active
          context.Fail("This token is expired. Please login again.");
          return;
        }


        if (context.SecurityToken is not JsonWebToken accessToken ||
            string.IsNullOrWhiteSpace(accessToken.EncodedToken) ||
            !await userSessionFactory.ValidateAccessTokenAsync(accessToken.EncodedToken)) {
          context.Fail("This token is not in our database.");
          return;
        }

        await userRepository.UpdateAsync(user, lastActiveAt: DateTimeOffset.UtcNow);
      },
      OnMessageReceived = context => {
        var accessToken = context.Request.Query["access_token"];
        var path = context.HttpContext.Request.Path;

        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/signalr")) {
          context.Token = accessToken;
        }

        return Task.CompletedTask;
      },
      OnChallenge = context => {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>()
          .CreateLogger(nameof(JwtBearerEvents));
        logger.LogError($"OnChallenge error {context.Error}, {context.ErrorDescription}");
        return Task.CompletedTask;
      },
    };
  }

  public void Configure(string? name, JwtBearerOptions options) {
    if (name == JwtBearerDefaults.AuthenticationScheme) Configure(options);
  }
}
