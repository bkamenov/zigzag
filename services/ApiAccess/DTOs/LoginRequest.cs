using System.ComponentModel.DataAnnotations;

namespace ApiAccess.DTOs;

public class LoginRequest
{
  [Required(ErrorMessage = "Username is required.")]
  [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters.")]
  public required string Username { get; set; }

  [Required(ErrorMessage = "Password is required.")]
  [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
  public required string Password { get; set; }
}