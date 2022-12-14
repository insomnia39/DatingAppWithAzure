using AutoMapper;
using DatingApp.BLL.Repository;
using DatingApp.DAL;
using DatingApp.DAL.DTO.Account;
using DatingApp.DAL.DTO.User;
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
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        //private readonly ILogger _log;
        private IMapper _map { get; set; }

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
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

            _mapper ??= mapper;
            _userRepository ??= userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var listUser = await _userRepository.GetUsersAsync();
                var dto = _mapper.Map<IEnumerable<UserDto>>(listUser);
                return new OkObjectResult(dto);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }

        [HttpGet("{username}")]
        public async Task<ActionResult> GetUser(string username)
        {
            try
            {
                var user = await _userRepository.GetUserByUsernameAsync(username);

                return new OkObjectResult(_mapper.Map<UserDto>(user));
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }
    }
}
