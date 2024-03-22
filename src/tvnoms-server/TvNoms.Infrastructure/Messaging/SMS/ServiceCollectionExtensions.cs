using Microsoft.Extensions.DependencyInjection;
using TvNoms.Core.Extensions.SmsSender;

namespace TvNoms.Infrastructure.Messaging.SMS;

public static class ServiceCollectionExtensions {
  public static IServiceCollection AddFakeSmsSender(this IServiceCollection services) {
    services.AddTransient<ISmsSender, FakeSMSSender>();
    return services;
  }
}
