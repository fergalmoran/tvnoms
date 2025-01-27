using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Primitives;
using TvNoms.Core.Entities;
using TvNoms.Core.Extensions.Identity;
using TvNoms.Core.Utilities.Validation;

namespace TvNoms.Infrastructure.Identity;

public class ClientContext : IClientContext {
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly UserManager<User> _userManager;

  public ClientContext(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager) {
    _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
  }

  public string? UserAgent {
    get {
      return _httpContextAccessor.HttpContext?.Request.Headers.UserAgent;
    }
  }

  public Guid? UserId {
    get {
      var user = _httpContextAccessor?.HttpContext?.User;
      if (user == null) return null;
      var userIdString = _userManager.GetUserId(user);
      return Guid.TryParse(userIdString, out Guid userId) ? userId : Guid.Empty;
    }
  }

  public string? DeviceId {
    get {
      if (!ValidationHelper.TryParseUserAgent(UserAgent, out var userAgent)) {
        return null;
      }

      var values = new object?[] {
        userAgent.Device,
        userAgent.UA,
        userAgent.OS,
        IpAddress
      };

      var deviceId = string
        .Join(",", values
          .Where((agent) => !string.IsNullOrEmpty(agent?.ToString()))).ToLower();
      deviceId = TokenHelper.GenerateMD5Hash(deviceId);
      return deviceId;
    }
  }

  public string? IpAddress {
    get {
      return _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress is IPAddress remoteIp
        ? remoteIp.Equals(IPAddress.IPv6Loopback) ? IPAddress.Loopback.ToString() : remoteIp.MapToIPv4().ToString()
        : null;
    }
  }

  public string? Issuer {
    get {
      HttpContext? context = _httpContextAccessor.HttpContext;
      Uri? issuer = context != null
        ? new Uri(string.Concat(context.Request.Scheme, "://", context.Request.Host.ToUriComponent()), UriKind.Absolute)
        : null;
      return issuer?.GetLeftPart(UriPartial.Authority) ??
             throw new InvalidOperationException("Unable to determine the issuer.");
    }
  }

  public string? Audience {
    get {
      HttpContext? context = _httpContextAccessor.HttpContext;
      Uri? audience = null;
      audience ??= context?.Request?.Headers?.Origin is StringValues origin && !StringValues.IsNullOrEmpty(origin)
        ? new Uri(origin.ToString(), UriKind.Absolute)
        : null;
      audience ??= context?.Request?.Headers?.Referer is StringValues referer && !StringValues.IsNullOrEmpty(referer)
        ? new Uri(referer.ToString(), UriKind.Absolute)
        : null;
      return audience?.GetLeftPart(UriPartial.Authority);
    }
  }
}
