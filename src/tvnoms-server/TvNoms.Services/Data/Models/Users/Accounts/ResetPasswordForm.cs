using System.Text.Json.Serialization;
using FluentValidation;
using TvNoms.Server.Services.Validation;

namespace TvNoms.Server.Services.Data.Models.Users.Accounts {
  public class ResetPasswordForm {
    public string Username { get; set; } = default!;

    [JsonIgnore] public ContactType UsernameType => ValidationHelper.GetContactType(Username);

    public string Password { get; set; } = default!;

    public string Code { get; set; } = default!;
  }

  public class ResetPasswordFormValidator : AbstractValidator<ResetPasswordForm> {
    public ResetPasswordFormValidator() {
      RuleFor(_ => _.Username).NotEmpty().Username();
      RuleFor(_ => _.Password).NotEmpty().Password();
      RuleFor(_ => _.Code).NotEmpty();
    }
  }
}
