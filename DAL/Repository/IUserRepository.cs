using DatingApp.DAL.Helper;
using DatingApp.DAL.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatingApp.DAL.Repository
{
    public interface IUserRepository
    {
        void Update(User user);
        Task<bool> SaveAllAsync();
        Task<PagedList<User>> GetUsersAsync(UserParams userParams);
        Task<User> GetUserByIdAsync(string id);
        Task<User> GetUserByUsernameAsync(string username);
        Task<List<User>> GetUserByIdAsync(List<string> listUserId);
    }
}
