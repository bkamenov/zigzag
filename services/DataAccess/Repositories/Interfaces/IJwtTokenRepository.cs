namespace DataAccess.Repositories;

public interface IJwtTokenRepository
{
  Task<bool> IsTokenValidAsync(string token);
  Task SaveTokenAsync(string accessToken, int lifetimeHours, string userId);
  Task<long> RemoveTokensAsync(string userId);
  Task RemoveExpiredTokensAsync();
}
