using DatingApp.DAL.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatingApp.BLL.UserManagement
{
    public interface IUserService
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task<List<User>> GetUserByIdAsync(List<string> listUserId);
    }
}
