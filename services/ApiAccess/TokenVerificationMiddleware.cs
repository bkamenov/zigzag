using System.Net;
using Microsoft.AspNetCore.Http;

public class TokenVerificationMiddleware
{
  private readonly RequestDelegate _next;
  private readonly IJwtService _jwtService;

  public TokenVerificationMiddleware(RequestDelegate next, IJwtService jwtService)
  {
    _next = next;
    _jwtService = jwtService;
  }

  public async Task InvokeAsync(HttpContext context)
  {
    var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");

    if (string.IsNullOrEmpty(token))
    {
      // Allow anonymous access
      await _next(context);
      return;
    }

    // Check token existence in the database
    var isValid = await _jwtService.ValidateTokenAsync(token);

    if (!isValid)
    {
      context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
      await context.Response.WriteAsync("Invalid or revoked token.");
      return;
    }

    // Proceed with the request
    await _next(context);
  }
}
