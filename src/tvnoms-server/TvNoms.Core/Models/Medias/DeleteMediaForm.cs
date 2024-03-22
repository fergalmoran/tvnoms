using FluentValidation;

namespace TvNoms.Core.Models.Medias;

public class DeleteMediaForm {
  public Guid Id { get; set; }
}

public class DeleteMediaFormValidator : AbstractValidator<DeleteMediaForm> {
  public DeleteMediaFormValidator() {
  }
}
