using DataAccess.Repositories;
using Microsoft.Extensions.Options;

public class TokenCleanupService : BackgroundService
{
  private readonly IJwtTokenRepository _tokenRepository;
  private readonly JwtSettings _jwtSettings;
  private readonly ILogger<TokenCleanupService> _logger;

  public TokenCleanupService(
      IJwtTokenRepository tokenRepository,
      IOptions<JwtSettings> jwtSettings,
      ILogger<TokenCleanupService> logger)
  {
    _tokenRepository = tokenRepository;
    _jwtSettings = jwtSettings.Value;
    _logger = logger;
  }

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    while (!stoppingToken.IsCancellationRequested)
    {
      try
      {
        var expirationTime = DateTime.UtcNow.AddHours(-_jwtSettings.TokenLifetimeHours);

        // Remove expired tokens
        await _tokenRepository.RemoveExpiredTokensAsync();

        _logger.LogInformation("Token cleanup completed successfully at {Time}", DateTime.UtcNow);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "An error occurred during token cleanup.");
      }

      // Wait for a minute before running again
      await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
    }
  }
}
