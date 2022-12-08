using DatingApp.DAL;
using DatingApp.DAL.DTO.Account;
using DatingApp.DAL.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static DatingApp.DAL.Model.UserProperty;

namespace DatingApp.FrontEndAPI.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly ProfileContext _context;

        public AccountController(ProfileContext context)
        {
            _context ??= context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterDto dto)
        {
            if (UsernameExist(dto.Username)) return new BadRequestObjectResult("Username already taken");

            using var hmac = new HMACSHA512();

            var user = new User
            {
                Username = dto.Username.ToLower(),
                Gender = dto.Gender.Equals(Gender.Male) ? Gender.Male : Gender.Female,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)),
                PasswordSalt = hmac.Key
            };

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<User>> Login(LoginRequestDto dto)
        {
            var user = await _context.User.SingleOrDefaultAsync(p => p.Username == dto.Username);

            if (user == null) return Unauthorized();

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized();
            }

            return user;
        }

        public bool UsernameExist(string username)
        {
            try
            {
                username = username.ToLower();
                return _context.User.ToList().Where(p => p.Username == username).Any();
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                throw;
            }
        }
    }
}
