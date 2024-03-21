﻿using FluentValidation;
using TvNoms.Server.Services.Validation;

namespace TvNoms.Server.Services.Data.Models.Users.Accounts;

public class ChangePasswordForm {
  public string CurrentPassword { get; set; } = default!;

  public string NewPassword { get; set; } = default!;

  public string ConfirmNewPassword { get; set; } = default!;
}

public class ChangePasswordFormValidator : AbstractValidator<ChangePasswordForm> {
  public ChangePasswordFormValidator() {
    RuleFor(_ => _.CurrentPassword).NotEmpty();
    RuleFor(_ => _.NewPassword).NotEmpty().Password();
    RuleFor(_ => _.ConfirmNewPassword).NotEmpty().Password().Equal(_ => _.NewPassword);
  }
}
