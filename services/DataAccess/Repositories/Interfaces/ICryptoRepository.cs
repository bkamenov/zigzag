namespace DataAccess.Repositories;

public interface ICryptoRepository
{
  Task<long> GetCryptosCountAsync(string search);
  Task<IEnumerable<Crypto>> GetCryptosAsync(string search, int page, int pageCount);
  Task AddCryptoAsync(Crypto crypto);
  Task RemoveCryptoAsync(string id);
}
