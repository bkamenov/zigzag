namespace ApiAccess.DTOs;

public class AuthResult
{
  public bool Success { get; set; }
  public string? Message { get; set; }
  public string? AccessToken { get; set; }
}