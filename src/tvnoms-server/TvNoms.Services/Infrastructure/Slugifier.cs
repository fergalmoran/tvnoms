using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace TvNoms.Server.Services.Infrastructure;

public static class Slugifier {
  public static async Task<string> GenerateSlugAsync(string text, Func<string, Task<bool>> exists,
    string separator = "-") {
    ArgumentNullException.ThrowIfNull(text);
    ArgumentNullException.ThrowIfNull(separator);

    string slug;
    int count = 1;

    do {
      slug = GenerateSlug($"{text}{(count == 1 ? "" : $" {count}")}".Trim(), separator);
      count += 1;
    } while (await exists(slug));

    return slug;
  }

  // URL Slugify algorithm in C#?
  // source: https://stackoverflow.com/questions/2920744/url-slugify-algorithm-in-c/2921135#2921135
  public static string GenerateSlug(string input, string separator = "-") {
    ArgumentNullException.ThrowIfNull(input);
    ArgumentNullException.ThrowIfNull(separator);

    static string RemoveDiacritics(string text) {
      var normalizedString = text.Normalize(NormalizationForm.FormD);
      var stringBuilder = new StringBuilder();

      foreach (var c in normalizedString) {
        var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
        if (unicodeCategory != UnicodeCategory.NonSpacingMark) {
          stringBuilder.Append(c);
        }
      }

      return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }

    // remove all diacritics.
    input = RemoveDiacritics(input);

    // Remove everything that's not a letter, number, hyphen, dot, whitespace or underscore.
    input = Regex.Replace(input, @"[^a-zA-Z0-9\-\.\s_]", string.Empty, RegexOptions.Compiled).Trim();

    // replace symbols with a hyphen.
    input = Regex.Replace(input, @"[\-\.\s_]", separator, RegexOptions.Compiled);

    // replace double occurrences of hyphen.
    input = Regex.Replace(input, @"(-){2,}", "$1", RegexOptions.Compiled).Trim('-');

    return input;
  }
}
