using TvNoms.Core.Extensions.SmsSender;

namespace TvNoms.Infrastructure.Messaging.SMS;

public class FakeSMSSender : ISmsSender {
  public Task SendAsync(string phoneNumber, string message, CancellationToken cancellationToken = default) {
    throw new NotImplementedException();
  }
}
