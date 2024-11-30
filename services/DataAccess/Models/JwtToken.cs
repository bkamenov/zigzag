
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAccess.Models;

public class JwtToken
{
  [BsonId]
  public required ObjectId Id { get; set; }
  [BsonElement("tokenValue")]
  public required string TokenValue { get; set; } // JWT Token as primary key
  [BsonElement("userId")]
  public required string UserId { get; set; } // Foreign key referencing User.Id
  [BsonElement("expiration")]
  public required DateTime Expiration { get; set; } // UTC timestamp when created
}
