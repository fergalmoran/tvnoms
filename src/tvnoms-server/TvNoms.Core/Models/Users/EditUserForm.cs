﻿using FluentValidation;
using TvNoms.Core.Entities;
using TvNoms.Core.Utilities.Validation;
using AbstractProfile = AutoMapper.Profile;

namespace TvNoms.Core.Models.Users;

public class EditUserForm {
  public string UserName { get; set; } = default!;

  public string FirstName { get; set; } = default!;

  public string LastName { get; set; } = default!;

  public string? Email { get; set; } = default!;

  public string? PhoneNumber { get; set; } = default!;

  public Guid? AvatarId { get; set; }

  public string? Bio { get; set; }
}

public class EditUserFormValidator : AbstractValidator<EditUserForm> {
  public EditUserFormValidator() {
    RuleFor(_ => _.FirstName).NotEmpty();
    RuleFor(_ => _.LastName).NotEmpty();
    RuleFor(_ => _.UserName).NotEmpty();
    RuleFor(_ => _.Email!).NotEmpty().Email().When(_ => !string.IsNullOrWhiteSpace(_.Email));
    RuleFor(_ => _.PhoneNumber!).PhoneNumber().When(_ => !string.IsNullOrWhiteSpace(_.PhoneNumber));
    RuleFor(_ => _.Bio);
  }
}

public class EditUserFormProfile : AbstractProfile {
  public EditUserFormProfile() {
    CreateMap<EditUserForm, User>();
  }
}
