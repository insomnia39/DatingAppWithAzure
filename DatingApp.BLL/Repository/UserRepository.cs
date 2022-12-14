using DatingApp.DAL;
using DatingApp.DAL.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatingApp.BLL.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ProfileContext _context;

        public UserRepository(ProfileContext context)
        {
            _context ??= context;
        }
        public async Task<User> GetUserByIdAsync(string id)
        {
            return await _context.User.FindAsync(id);
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.User.SingleOrDefaultAsync(x => x.Username == username);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.User.ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }
    }
}
