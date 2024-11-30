using DataAccess.Models;

namespace DataAccess.Repositories;

public interface IUserRepository
{
  Task<User?> GetUserByCredentialsAsync(string username, string password);
  Task<User?> GetUserByUsernameAsync(string username);
  Task CreateUserAsync(User user);
}
