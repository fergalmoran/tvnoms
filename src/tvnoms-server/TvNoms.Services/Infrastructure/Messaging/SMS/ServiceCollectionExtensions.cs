using Microsoft.Extensions.DependencyInjection;

namespace TvNoms.Server.Services.Infrastructure.Messaging.SMS;

public static class ServiceCollectionExtensions {
  public static IServiceCollection AddFakeSmsSender(this IServiceCollection services) {
    services.AddTransient<ISmsSender, FakeSMSSender>();
    return services;
  }
}
