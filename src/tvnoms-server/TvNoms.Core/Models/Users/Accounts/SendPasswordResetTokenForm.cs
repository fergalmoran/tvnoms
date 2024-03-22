using System.Text.Json.Serialization;
using FluentValidation;
using TvNoms.Core.Utilities.Validation;

namespace TvNoms.Core.Models.Users.Accounts;

public class SendPasswordResetTokenForm {
  public string Username { get; set; } = default!;

  [JsonIgnore] public ContactType UsernameType => ValidationHelper.GetContactType(Username);
}

public class SendPasswordResetTokenFormValidator : AbstractValidator<SendPasswordResetTokenForm> {
  public SendPasswordResetTokenFormValidator() {
    RuleFor(_ => _.Username).NotEmpty().Username();
  }
}
