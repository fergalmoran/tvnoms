using FluentValidation;
using TvNoms.Server.Services.Validation;

namespace TvNoms.Server.Services.Data.Models.Users.Accounts {
  public class VerifyUsernameForm {
    public string Username { get; set; } = default!;

    public ContactType UsernameType { get; set; }

    public string Code { get; set; } = default!;
  }

  public class VerifyUsernameFormValidator : AbstractValidator<VerifyUsernameForm> {
    public VerifyUsernameFormValidator() {
      RuleFor(_ => _.Username).NotEmpty().Username();
      RuleFor(_ => _.Code).NotEmpty();
    }
  }
}
