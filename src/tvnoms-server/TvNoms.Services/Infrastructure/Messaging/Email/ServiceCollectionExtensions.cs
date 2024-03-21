using Microsoft.Extensions.DependencyInjection;

namespace TvNoms.Server.Services.Infrastructure.Messaging.Email;

public static class ServiceCollectionExtensions {
  public static IServiceCollection AddMailgunEmailSender(this IServiceCollection services,
    Action<MailgunEmailOptions> options) {
    services.Configure(options);
    services.AddMailgunEmailSender();
    return services;
  }

  public static IServiceCollection AddMailgunEmailSender(this IServiceCollection services) {
    services.AddTransient<IEmailSender, MailgunEmailSender>();
    return services;
  }
}
