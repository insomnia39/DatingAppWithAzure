using DatingApp.BLL.JWT;
using DatingApp.DAL;
using DatingApp.DAL.DTO.Account;
using DatingApp.DAL.DTO.User;
using DatingApp.DAL.Model;
using Microsoft.AspNetCore.Mvc;
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
        private readonly ITokenService _tokenService;

        public AccountController(ProfileContext context, ITokenService tokenService)
        {
            _tokenService ??= tokenService;
            _context ??= context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserLoginDto>> Register(RegisterDto dto)
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

            return new UserLoginDto { Username = user.Username, Token = _tokenService.CreateToken(user), PhotoUrl = user.GetPhotoUrl() };
        }

        [HttpPost("Login")]
        public ActionResult<UserLoginDto> Login(LoginRequestDto dto)
        {
            var users = _context.User.Where(p => p.Username == dto.Username.ToLower()).ToList();

            if (users.Count != 1) return new BadRequestObjectResult("Invalid username or password");

            var user = users.First();

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return new BadRequestObjectResult("Invalid username or password");
            }

            return new UserLoginDto { Username = user.Username, Token = _tokenService.CreateToken(user), PhotoUrl = user.GetPhotoUrl()};
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
