using FluentValidation;

namespace TvNoms.Server.Services.Data.Models.Users.Accounts;

public class RefreshSessionForm {
  public string RefreshToken { get; set; } = default!;
}

public class RefreshSessionFormValidator : AbstractValidator<RefreshSessionForm> {
  public RefreshSessionFormValidator() {
    RuleFor(_ => _.RefreshToken).NotEmpty();
  }
}
