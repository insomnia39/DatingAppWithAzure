using DatingApp.DAL.Helper;
using DatingApp.DAL.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.DAL.Repository
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

        public async Task<PagedList<User>> GetUsersAsync(UserParams userParams)
        {
            var minDob = DateTime.Today.AddYears(-userParams.MaxAge);
            var maxDob = DateTime.Today.AddYears(-userParams.MinAge);
            var query = _context.User.AsQueryable();
            query = query.Where(p => p.Username != userParams.CurrentUsername);
            query = query.Where(p => p.Gender == userParams.Gender);

            query = query.Where(p => p.DateOfBirth >= minDob && p.DateOfBirth <= maxDob);
            query = userParams.OrderBy switch
            {
                "created" => query.OrderByDescending(p => p.Created),
                _ => query.OrderByDescending(p => p.LastActive)
            };
            return await PagedList<User>.CreateAsync(query.AsNoTracking(), userParams.PageNumber, userParams.PageSize);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        public async Task<List<User>> GetUserByIdAsync(List<string> listUserId)
        {
            return await _context.User.Where(p => listUserId.Contains(p.Id)).ToListAsync();
        }
    }
}
