using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DataAccess.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using DataAccess.Models;

public class JwtService : IJwtService
{
  private readonly JwtSettings _jwtSettings;
  private readonly IJwtTokenRepository _jwtTokenRepository;

  public JwtService(IOptions<JwtSettings> jwtSettings, IJwtTokenRepository jwtTokenRepository)
  {
    _jwtSettings = jwtSettings.Value;
    _jwtTokenRepository = jwtTokenRepository;
  }

  /// <summary>
  /// Generates a JWT access token for the specified user.
  /// </summary>
  public string GenerateAccessToken(User user)
  {
    var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };

    claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: _jwtSettings.Issuer,
        audience: _jwtSettings.Audience,
        claims: claims,
        expires: DateTime.UtcNow.AddHours(_jwtSettings.TokenLifetimeHours),
        signingCredentials: creds);

    return new JwtSecurityTokenHandler().WriteToken(token);
  }

  /// <summary>
  /// Validates a JWT token and checks if it exists in the database.
  /// </summary>
  public async Task<bool> ValidateTokenAsync(string token)
  {
    return await _jwtTokenRepository.IsTokenValidAsync(token);
  }
}
