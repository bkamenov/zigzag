using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiAccess.DTOs;
using System.Security.Claims;

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
  /// Retrieves the username and the user roles for the currently logged user.
  /// </summary>
  /// <returns>Object {username, roles[]} if successful</returns>
  [HttpGet("loginInfo")]
  [Authorize]
  public IActionResult loginInfo()
  {
    var username = User.FindFirst(ClaimTypes.Name)?.Value;
    if (string.IsNullOrEmpty(username))
    {
      return Unauthorized(new
      {
        Message = "Username claim is not present."
      });
    }

    var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
    if (roles.Count == 0)
    {
      return Unauthorized(new
      {
        Message = "No role claima are present."
      });
    }

    return Ok(new
    {
      username,
      roles
    });
  }

  /// <summary>
  /// Handles user logout.
  /// </summary>
  /// <returns>A success message if the token is invalidated.</returns>
  [HttpPost("logout")]
  [Authorize]
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
