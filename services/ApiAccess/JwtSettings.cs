public class JwtSettings
{
  public JwtSettings()
  {
    Secret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? string.Empty;
  }

  public string Secret { get; init; }
  public required string Issuer { get; set; }
  public required string Audience { get; set; }
  public required int TokenLifetimeHours { get; set; }
}
