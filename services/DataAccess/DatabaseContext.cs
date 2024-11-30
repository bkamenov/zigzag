using MongoDB.Driver;

namespace DataAccess;

public class DatabaseContext
{
  private readonly IMongoDatabase _database;

  public DatabaseContext(string connectionString, string databaseName)
  {
    var client = new MongoClient(connectionString);
    _database = client.GetDatabase(databaseName);
  }

  public IMongoDatabase GetDatabase() => _database;
}
