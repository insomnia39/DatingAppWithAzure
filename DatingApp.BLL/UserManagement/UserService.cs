using DatingApp.DAL.Model;
using DatingApp.DAL.UnitOfWork;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatingApp.BLL.UserManagement
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;

        public UserService(IUnitOfWork uow)
        {
            _uow ??= uow;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _uow.UserRepository.GetUserByUsernameAsync(username);
        }

        public async Task<List<User>> GetUserByIdAsync(List<string> listUserId)
        {
            return await _uow.UserRepository.GetUserByIdAsync(listUserId);
        }
    }
}
