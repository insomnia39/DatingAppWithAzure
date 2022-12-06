using AutoMapper;
using DatingApp.DAL;
using DatingApp.DAL.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using static DatingApp.DAL.Model.User;

namespace DatingApp.FrontEndAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        //private readonly ILogger _log;
        private readonly ProfileContext _context;
        private IMapper _map { get; set; }

        public UserController(ProfileContext context)
        {
            //_log ??= logger;
            if (_map == null)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<User, UserDto>();
                    cfg.CreateMap<UserDto, User>();
                });

                _map = config.CreateMapper();
            }

            _context ??= context;
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var listUser = await _context.User.ToListAsync();
                var dto = JsonConvert.SerializeObject(listUser);
                return new OkObjectResult(dto);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDto dto)
        {
            try
            {
                var newUser = _map.Map<User>(dto);
                await _context.User.AddAsync(newUser);
                await _context.SaveChangesAsync();
                return new CreatedResult("User", newUser);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }
    }

    public class UserDto
    {
        public string Username { get; set; }
        public string Gender  { get; set; }
    }
}
