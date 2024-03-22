namespace TvNoms.Core.Extensions.SmsSender;

public interface ISmsSender {
  Task SendAsync(string phoneNumber, string message, CancellationToken cancellationToken = default);
}
