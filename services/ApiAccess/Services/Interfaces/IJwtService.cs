using System.Security.Claims;
using DataAccess.Models;

public interface IJwtService
{
  string GenerateAccessToken(User user);
  Task<bool> ValidateTokenAsync(string token);
}

