using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TvNoms.Server.Services.Infrastructure.Messaging.Email;

public interface IEmailSender {
  Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default);
}

public class MailgunEmailSender(IOptions<MailgunEmailOptions> emailSettings, ILogger<MailgunEmailSender> logger)
  : IEmailSender {
  private readonly MailgunEmailOptions _emailSettings = emailSettings.Value;

  public async Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default) {
    using var client = new HttpClient { BaseAddress = new Uri(_emailSettings.ApiBaseUri) };
    client.DefaultRequestHeaders.Authorization =
      new AuthenticationHeaderValue("Basic",
        Convert.ToBase64String(Encoding.ASCII.GetBytes(_emailSettings.ApiKey)));


    foreach (var recipient in message.Recipients) {
      logger.LogInformation(
        @"From: {EmailSettingsFrom}\nTo: {AccountEmail}\nApi key:{EmailSettingsApiKey}",
        _emailSettings.From,
        recipient,
        _emailSettings.ApiKey);

      var content = new FormUrlEncodedContent(new[] {
        new KeyValuePair<string, string>("from", _emailSettings.From),
        new KeyValuePair<string, string>("to", recipient),
        new KeyValuePair<string, string>("subject", message.Subject),
        new KeyValuePair<string, string>("html", message.Body)
      });

      var result = await client.PostAsync(_emailSettings.RequestUri, content, cancellationToken);
      if (result.StatusCode != HttpStatusCode.OK) {
        logger.LogError(
          "Error {ResultStatusCode} sending mail\\n{ResultReasonPhrase}",
          result.StatusCode,
          result.ReasonPhrase);
      }
    }
  }
}
