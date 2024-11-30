using ApiAccess.DTOs;

public interface IAuthService
{
  Task<AuthResult> LoginAsync(LoginRequest request);
  Task<AuthResult> LogoutAsync(string userId);
}
