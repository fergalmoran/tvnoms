namespace TvNoms.Server.Services.Infrastructure.Messaging.Email;

public class MailgunEmailOptions {
  public string ApiKey { get; set; }
  public string ApiBaseUri { get; set; }
  public string RequestUri { get; set; }
  public string From { get; set; }
}
