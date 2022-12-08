using AutoMapper;
using DatingApp.DAL;
using DatingApp.DAL.DTO.Account;
using DatingApp.DAL.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatingApp.FrontEndAPI.Controllers
{
    public class UserController : BaseApiController
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
                    cfg.CreateMap<User, UserRequestDto>();
                    cfg.CreateMap<UserRequestDto, User>();
                    cfg.CreateMap<User, UserResponseDto>();
                    cfg.CreateMap<UserResponseDto, User>();
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
                var dto = _map.Map<List<UserResponseDto>>(listUser);
                return new OkObjectResult(dto);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserRequestDto dto)
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
}
