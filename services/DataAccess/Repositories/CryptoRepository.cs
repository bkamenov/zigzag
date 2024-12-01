using MongoDB.Driver;

namespace DataAccess.Repositories;

public class CryptoRepository : ICryptoRepository
{
  private readonly IMongoCollection<Crypto> _cryptos;

  public CryptoRepository(IMongoDatabase database)
  {
    _cryptos = database.GetCollection<Crypto>("cryptos");
  }

  public async Task<long> GetCryptosCountAsync(string search)
  {
    // If search is null or empty, use an empty filter
    var filter = string.IsNullOrEmpty(search)
        ? Builders<Crypto>.Filter.Empty
        : Builders<Crypto>.Filter.Regex(c => c.Name, new MongoDB.Bson.BsonRegularExpression(search, "i"));

    // Count matching documents
    var count = await _cryptos.CountDocumentsAsync(filter);
    return count;
  }

  public async Task<IEnumerable<Crypto>> GetCryptosAsync(string search, int page, int pageCount)
  {
    var filter = string.IsNullOrEmpty(search)
        ? Builders<Crypto>.Filter.Empty
        : Builders<Crypto>.Filter.Regex(c => c.Name, new MongoDB.Bson.BsonRegularExpression(search, "i"));

    var options = new FindOptions
    {
      Collation = new Collation("en", strength: CollationStrength.Primary), // Case-insensitive collation (а как е за уникод?)
    };

    return await _cryptos.Find(filter, options)
        .SortBy(c => c.Name)
        .Skip((page - 1) * pageCount)
        .Limit(pageCount)
        .ToListAsync();
  }

  public async Task AddCryptoAsync(Crypto crypto)
  {
    await _cryptos.InsertOneAsync(crypto);
  }

  public async Task RemoveCryptoAsync(string id)
  {
    await _cryptos.DeleteOneAsync(c => c.Id == id);
  }
}
