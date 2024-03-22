using System.Text.Json.Serialization;
using FluentValidation;
using TvNoms.Core.Utilities.Validation;

namespace TvNoms.Core.Models.Users.Accounts {
  public class SendUsernameTokenForm {
    public string Username { get; set; } = default!;

    [JsonIgnore] public ContactType UsernameType => ValidationHelper.GetContactType(Username);
  }

  public class SendUsernameTokenFormValidator : AbstractValidator<SendUsernameTokenForm> {
    public SendUsernameTokenFormValidator() {
      RuleFor(_ => _.Username).NotEmpty().Username();
    }
  }
}
