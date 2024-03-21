using System.Text.Json.Serialization;
using FluentValidation;
using TvNoms.Server.Services.Validation;

namespace TvNoms.Server.Services.Data.Models.Users.Accounts;

public class SendPasswordResetTokenForm {
  public string Username { get; set; } = default!;

  [JsonIgnore] public ContactType UsernameType => ValidationHelper.GetContactType(Username);
}

public class SendPasswordResetTokenFormValidator : AbstractValidator<SendPasswordResetTokenForm> {
  public SendPasswordResetTokenFormValidator() {
    RuleFor(_ => _.Username).NotEmpty().Username();
  }
}
