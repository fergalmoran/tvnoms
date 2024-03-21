using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using DeviceId;

namespace TvNoms.Server.Services.Identity;

public class TokenHelper {
  public static string Secret => GenerateSHA256Hash(
    new DeviceIdBuilder().AddMachineName().AddOsVersion().AddUserName()
      .AddFileToken(Path.ChangeExtension(
        Assembly.GetEntryAssembly()!.Location,
        nameof(Secret).ToLower())).ToString());

  public static string GenerateSHA256Hash(string input) {
    using var sha256 = SHA256.Create();
    byte[] inputBytes = Encoding.UTF8.GetBytes(input);
    byte[] hashBytes = sha256.ComputeHash(inputBytes);

    var hashBuilder = new StringBuilder();
    foreach (var t in hashBytes) {
      hashBuilder.Append(t.ToString("x2"));
    }

    return hashBuilder.ToString();
  }

  public static string GenerateMD5Hash(string input) {
    using MD5 md5 = MD5.Create();
    byte[] inputBytes = Encoding.UTF8.GetBytes(input);
    byte[] hashBytes = md5.ComputeHash(inputBytes);

    var sb = new StringBuilder();
    foreach (var t in hashBytes) {
      sb.Append(t.ToString("x2")); // Convert byte to hexadecimal
    }

    return sb.ToString();
  }

  public const string NATURAL_NUMERIC_CHARS = "123456789";
  public const string WHOLE_NUMERIC_CHARS = "0123456789";
  public const string LOWER_ALPHA_CHARS = "abcdefghijklmnopqrstuvwyxz";
  public const string UPPER_ALPHA_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

  // How can I generate random alphanumeric strings?
  // source: https://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings
  public static string GenerateText(int size, string characters) {
    ArgumentNullException.ThrowIfNull(characters);

    var charArray = characters.ToCharArray();
    byte[] data = new byte[4 * size];
    using (var crypto = RandomNumberGenerator.Create()) {
      crypto.GetBytes(data);
    }

    StringBuilder result = new StringBuilder(size);
    for (int i = 0; i < size; i++) {
      var rnd = BitConverter.ToUInt32(data, i * 4);
      var idx = rnd % charArray.Length;

      result.Append(charArray[idx]);
    }

    return result.ToString();
  }

  public static string GenerateStamp() {
    static byte[] Generate128BitsOfRandomEntropy() {
      var randomBytes = new byte[16]; // 16 Bytes will give us 128 bits.
      using var rngCsp = RandomNumberGenerator.Create();
      // Fill the array with cryptographically secure random bytes.
      rngCsp.GetBytes(randomBytes);

      return randomBytes;
    }

    return new Guid(Generate128BitsOfRandomEntropy()).ToString().Replace("-", "", StringComparison.Ordinal);
  }
}
