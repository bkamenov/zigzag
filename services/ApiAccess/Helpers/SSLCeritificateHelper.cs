using System.Security.Cryptography.X509Certificates;

namespace ApiAccess.Helpers;

public static class SSLCertificateHelper
{
  /// <summary>
  /// Loads an X509Certificate2 from .cer and .key files using CreateFromPemFile.
  /// </summary>
  public static X509Certificate2 LoadCertificate(string cerFilePath, string keyFilePath)
  {
    // Load the certificate from the .cer file
    var certificate = X509Certificate2.CreateFromPemFile(cerFilePath, keyFilePath);

    return certificate;
  }
}