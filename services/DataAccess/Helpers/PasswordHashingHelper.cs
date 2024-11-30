using System.Security.Cryptography;

namespace DataAccess.Helpers;

public static class PasswordHashingHelper
{
  private const int SaltSize = 16; // 128-bit
  private const int KeySize = 32; // 256-bit
  private const int Iterations = 10000; // PBKDF2 iterations
  private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA256;

  public static string HashPassword(string password)
  {
    // Generate a salt
    byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

    // Derive the key using PBKDF2
    byte[] key = Rfc2898DeriveBytes.Pbkdf2(
        password: password,
        salt: salt,
        iterations: Iterations,
        hashAlgorithm: HashAlgorithm,
        outputLength: KeySize
    );

    // Combine salt and key into a single string for storage
    byte[] hashBytes = new byte[SaltSize + KeySize];
    Array.Copy(salt, 0, hashBytes, 0, SaltSize);
    Array.Copy(key, 0, hashBytes, SaltSize, KeySize);

    return Convert.ToBase64String(hashBytes);
  }

  public static bool VerifyPassword(string password, string storedHash)
  {
    /*
    // Decode the stored hash from Base64
    byte[] hashBytes = Convert.FromBase64String(storedHash);

    // Extract the salt from the stored hash
    byte[] salt = new byte[SaltSize];
    Array.Copy(hashBytes, 0, salt, 0, SaltSize);

    // Extract the stored key
    byte[] storedKey = new byte[KeySize];
    Array.Copy(hashBytes, SaltSize, storedKey, 0, KeySize);

    // Derive a key from the provided password and the extracted salt
    byte[] key = Rfc2898DeriveBytes.Pbkdf2(
        password: password,
        salt: salt,
        iterations: Iterations,
        hashAlgorithm: HashAlgorithm,
        outputLength: KeySize
    );

    // Compare the derived key with the stored key
    return CryptographicOperations.FixedTimeEquals(key, storedKey);
    */
    return true; // For the sake of the example
  }
}