using DataAccess.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DataAccess.Repositories;

public class JwtTokenRepository : IJwtTokenRepository
{
  private readonly IMongoCollection<JwtToken> _tokenCollection;

  public JwtTokenRepository(IMongoDatabase database)
  {
    _tokenCollection = database.GetCollection<JwtToken>("tokens");
  }

  public async Task<bool> IsTokenValidAsync(string token)
  {
    var existingToken = await _tokenCollection.Find(t => t.TokenValue == token && t.Expiration > DateTime.UtcNow).FirstOrDefaultAsync();
    return existingToken != null;
  }

  public async Task SaveTokenAsync(string accessToken, int lifetimeHours, string userId)
  {
    var tokenDocument = new JwtToken
    {
      Id = ObjectId.GenerateNewId(),
      TokenValue = accessToken,
      UserId = userId,
      Expiration = DateTime.UtcNow.AddHours(lifetimeHours)
    };

    await _tokenCollection.InsertOneAsync(tokenDocument);
  }

  public async Task<long> RemoveTokensAsync(string userId)
  {
    var deleteResult = await _tokenCollection.DeleteManyAsync(t => t.UserId == userId);
    return deleteResult.DeletedCount;
  }

  public async Task RemoveExpiredTokensAsync()
  {
    await _tokenCollection.DeleteManyAsync(t => t.Expiration <= DateTime.UtcNow);
  }
}