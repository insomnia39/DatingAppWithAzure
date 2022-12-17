using AutoMapper;
using DatingApp.BLL.JWT;
using DatingApp.DAL;
using DatingApp.DAL.DTO.Account;
using DatingApp.DAL.DTO.User;
using DatingApp.DAL.Model;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
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
        private readonly IMapper _mapper;

        public AccountController(ProfileContext context, ITokenService tokenService, IMapper mapper)
        {
            _tokenService ??= tokenService;
            _context ??= context;
            _mapper ??= mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserLoginDto>> Register(RegisterDto dto)
        {
            if (UsernameExist(dto.Username)) return new BadRequestObjectResult("Username already taken");

            var user = _mapper.Map<User>(dto);

            using var hmac = new HMACSHA512();

            user.Username = dto.Username.ToLower();
            user.Gender = dto.Gender.Equals(Gender.Male) ? Gender.Male : Gender.Female;
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));
            user.PasswordSalt = hmac.Key;

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            var returnDto = _mapper.Map<UserLoginDto>(user);

            returnDto.Token = _tokenService.CreateToken(user);
            return returnDto;
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

            var returnDto = _mapper.Map<UserLoginDto>(user);
            returnDto.Token = _tokenService.CreateToken(user);
            return returnDto;
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
