using DatingApp.DAL.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatingApp.BLL.Repository
{
    public interface IUserRepository
    {
        void Update(User user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(string id);
        Task<User> GetUserByUsernameAsync(string username);
    }
}
