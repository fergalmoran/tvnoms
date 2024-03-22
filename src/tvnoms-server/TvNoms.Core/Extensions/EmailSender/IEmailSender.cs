namespace TvNoms.Core.Extensions.EmailSender;

public interface IEmailSender {
  Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default);
}
