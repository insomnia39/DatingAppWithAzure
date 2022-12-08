using AutoMapper;
using DatingApp.DAL;
using DatingApp.DAL.DTO.Account;
using DatingApp.DAL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatingApp.FrontEndAPI.Controllers
{
    [Authorize]
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

        [AllowAnonymous]
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

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            try
            {
                return await _context.User.FindAsync(id);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }
    }
}
