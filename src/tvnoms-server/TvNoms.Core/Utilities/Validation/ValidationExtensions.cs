using FluentValidation;
using FluentValidation.Results;

namespace TvNoms.Core.Utilities.Validation;

public static class ValidationExtensions {
  public static IRuleBuilderOptionsConditions<T, string> Username<T>(this IRuleBuilder<T, string> ruleBuilder) {
    return ruleBuilder.Custom((value, context) => {
      var contactType = ValidationHelper.GetContactType(value);

      if (contactType == ContactType.Email) {
        if (!ValidationHelper.TryParseEmail(value, out var _))
          context.AddFailure($"'Email address' is not valid.");
      } else if (contactType == ContactType.PhoneNumber) {
        if (!ValidationHelper.TryParsePhoneNumber(value, out var _))
          context.AddFailure($"'Phone number' is not valid.");
      } else {
        throw new InvalidOperationException($"Input '{value}' was not recognized as a valid email or phone number.");
      }
    });
  }

  public static IRuleBuilderOptionsConditions<T, string> Email<T>(this IRuleBuilder<T, string> ruleBuilder) {
    return ruleBuilder.Custom((value, context) => {
      if (!ValidationHelper.TryParseEmail(value, out var _))
        context.AddFailure($"'Email address' is not valid.");
    });
  }

  public static IRuleBuilderOptionsConditions<T, string> PhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder) {
    return ruleBuilder.Custom((value, context) => {
      if (!ValidationHelper.TryParsePhoneNumber(value, out var _))
        context.AddFailure($"'Phone number' is not valid.");
    });
  }


  // How can I create strong passwords with FluentValidation?
  // source: https://stackoverflow.com/questions/63864594/how-can-i-create-strong-passwords-with-fluentvalidation
  public static IRuleBuilder<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder, int minimumLength = 6) {
    var options = ruleBuilder
      .MinimumLength(minimumLength)
      .Matches("[A-Z]").WithMessage("'{PropertyName}' must contain at least 1 upper case.")
      .Matches("[a-z]").WithMessage("'{PropertyName}' must contain at least 1 lower case.")
      .Matches("[0-9]").WithMessage("'{PropertyName}' must contain at least 1 digit.")
      .Matches("[^a-zA-Z0-9]").WithMessage("'{PropertyName}' must contain at least 1 special character.");

    return options;
  }

  public static IDictionary<string, string[]> ToDictionary(this IEnumerable<ValidationFailure> errors) {
    return errors
      .GroupBy(x => x.PropertyName)
      .ToDictionary(
        g => g.Key,
        g => g.Select(x => x.ErrorMessage).ToArray()
      );
  }
}
