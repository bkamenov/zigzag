using Microsoft.Extensions.Options;
using ApiAccess.DTOs;
using DataAccess.Helpers;
using DataAccess.Repositories;

public class AuthService : IAuthService
{
  private readonly IUserRepository _userRepository;
  private readonly IJwtTokenRepository _jwtTokenRepository;
  private readonly IJwtService _jwtService;
  private readonly JwtSettings _jwtSettings;

  public AuthService(
      IUserRepository userRepository,
      IJwtTokenRepository jwtTokenRepository,
      IJwtService jwtService,
      IOptions<JwtSettings> jwtSettings)
  {
    _userRepository = userRepository;
    _jwtTokenRepository = jwtTokenRepository;
    _jwtService = jwtService;
    _jwtSettings = jwtSettings.Value;
  }

  /// <summary>
  /// Logs in a user and generates a JWT access token.
  /// </summary>
  public async Task<AuthResult> LoginAsync(LoginRequest request)
  {
    var user = await _userRepository.GetUserByUsernameAsync(request.Username);
    if (user == null || !PasswordHashingHelper.VerifyPassword(request.Password, user.PasswordHash))
    {
      return new AuthResult
      {
        Success = false,
        Message = "Invalid username or password."
      };
    }

    var accessToken = _jwtService.GenerateAccessToken(user);

    // Remove previously generated tokens for this user
    await _jwtTokenRepository.RemoveTokensAsync(user.Id);

    // Save the token in the database
    await _jwtTokenRepository.SaveTokenAsync(accessToken, _jwtSettings.TokenLifetimeHours, user.Id);

    return new AuthResult
    {
      Success = true,
      AccessToken = accessToken
    };
  }

  /// <summary>
  /// Logs out a user by invalidating the token.
  /// </summary>
  public async Task<AuthResult> LogoutAsync(string userId)
  {
    var isRemoved = await _jwtTokenRepository.RemoveTokensAsync(userId);
    if (isRemoved == 0)
    {
      return new AuthResult
      {
        Success = false,
        Message = "Invalid token or already logged out."
      };
    }

    return new AuthResult
    {
      Success = true,
      Message = "Logged out successfully."
    };
  }
}
