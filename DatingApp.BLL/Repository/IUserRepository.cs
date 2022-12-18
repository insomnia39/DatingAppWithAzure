using DatingApp.BLL.Helpers;
using DatingApp.DAL.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatingApp.BLL.Repository
{
    public interface IUserRepository
    {
        void Update(User user);
        Task<bool> SaveAllAsync();
        Task<PagedList<User>> GetUsersAsync(UserParams userParams);
        Task<User> GetUserByIdAsync(string id);
        Task<User> GetUserByUsernameAsync(string username);
    }
}
