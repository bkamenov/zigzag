using MongoDB.Driver;
using DataAccess.Models;
using DataAccess.Helpers;
namespace DataAccess.Repositories;

public class UserRepository : IUserRepository
{
  private readonly IMongoCollection<User> _userCollection;

  public UserRepository(IMongoDatabase database)
  {
    _userCollection = database.GetCollection<User>("users");
  }

  public async Task<User?> GetUserByCredentialsAsync(string username, string password)
  {
    var user = await _userCollection.Find(u => u.Username == username).FirstOrDefaultAsync();
    if (user != null && PasswordHashingHelper.VerifyPassword(password, user.PasswordHash))
    {
      return user;
    }
    return null;
  }

  public async Task<User?> GetUserByUsernameAsync(string username)
  {
    return await _userCollection.Find(u => u.Username == username).FirstOrDefaultAsync();
  }

  public async Task CreateUserAsync(User user)
  {
    await _userCollection.InsertOneAsync(user);
  }
}
