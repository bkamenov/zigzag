using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiAccess.DTOs;
using System.Security.Claims;


[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
  private readonly IAuthService _authService;

  public AuthController(IAuthService authService)
  {
    _authService = authService;
  }

  /// <summary>
  /// Handles user login.
  /// </summary>
  /// <param name="request">The login request containing username and password.</param>
  /// <returns>JWT access token if successful.</returns>
  [HttpPost("login")]
  [AllowAnonymous]
  public async Task<IActionResult> Login([FromBody] LoginRequest request)
  {
    var result = await _authService.LoginAsync(request);
    if (!result.Success)
    {
      return Unauthorized(new
      {
        result.Message
      });
    }

    return Ok(new
    {
      result.AccessToken
    });
  }

  /// <summary>
  /// Handles user logout.
  /// </summary>
  /// <returns>A success message if the token is invalidated.</returns>
  [HttpPost("logout")]
  [Authorize] // Requires a valid JWT token to log out
  public async Task<IActionResult> Logout()
  {
    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (string.IsNullOrEmpty(userId))
    {
      return Unauthorized(new
      {
        Message = "userID claim is not present."
      });
    }

    var result = await _authService.LogoutAsync(userId);
    if (!result.Success)
    {
      return BadRequest(new
      {
        result.Message
      });
    }

    return Ok(new
    {
      Message = "Logged out successfully."
    });
  }
}
