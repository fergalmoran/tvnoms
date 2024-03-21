using System.Text.Json.Serialization;
using FluentValidation;
using TvNoms.Server.Services.Validation;

namespace TvNoms.Server.Services.Data.Models.Users.Accounts {
  public class SignUpForm {
    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string Username { get; set; } = default!;

    [JsonIgnore] public ContactType UsernameType => ValidationHelper.GetContactType(Username);

    public string Password { get; set; } = default!;
  }

  public class SignUpFormValidator : AbstractValidator<SignUpForm> {
    public SignUpFormValidator() {
      RuleFor(_ => _.FirstName).NotEmpty();
      RuleFor(_ => _.LastName).NotEmpty();
      RuleFor(_ => _.Username).NotEmpty().Username();
      RuleFor(_ => _.Password).NotEmpty().Password();
    }
  }
}
