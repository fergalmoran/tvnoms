using FluentValidation;

namespace TvNoms.Server.Services.Data.Models.Users.Accounts;

public class SignOutForm {
  public string RefreshToken { get; set; } = default!;
}

public class SignOutFormValidator : AbstractValidator<SignOutForm> {
  public SignOutFormValidator() {
    RuleFor(_ => _.RefreshToken).NotEmpty();
  }
}
