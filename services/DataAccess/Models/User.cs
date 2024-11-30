
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAccess.Models;

public class User
{
  [BsonId] // Marks this as the primary key for MongoDB
  [BsonRepresentation(BsonType.ObjectId)] // Allows storing ObjectId as a string
  public required string Id { get; set; }

  [BsonElement("username")] // Maps this field to "username" in the MongoDB document
  public required string Username { get; set; }

  [BsonElement("passwordHash")] // Securely stores the password hash
  public required string PasswordHash { get; set; }

  [BsonElement("roles")] // Maps this field to "roles" in MongoDB
  public required List<string> Roles { get; set; }
}
