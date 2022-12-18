using AutoMapper;
using DatingApp.BLL.Helpers;
using DatingApp.BLL.Photo;
using DatingApp.BLL.Repository;
using DatingApp.DAL.DTO.Account;
using DatingApp.DAL.DTO.User;
using DatingApp.DAL.Extensions;
using DatingApp.DAL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DatingApp.FrontEndAPI.Controllers
{
    [Authorize]
    public class UserController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        private readonly IPhotoRepository _photoRepository;

        //private readonly ILogger _log;
        private IMapper _map { get; set; }

        public UserController(IUserRepository userRepository, IPhotoRepository photoRepository, IMapper mapper, IPhotoService photoService)
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
            _photoService ??= photoService;
            _photoRepository ??= photoRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetUser([FromQuery] UserParams userParams)
        {
            try
            {
                var currentUser = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
                userParams.CurrentUsername = currentUser.Username;
                if (string.IsNullOrEmpty(userParams.Gender))
                {
                    userParams.Gender = currentUser.Gender == UserProperty.Gender.Female
                    ? UserProperty.Gender.Male
                    : UserProperty.Gender.Female;
                }

                var listUser = await _userRepository.GetUsersAsync(userParams);
                Response.AddPaginationHeader(new PaginationHeader(listUser.CurrentPage, listUser.PageSize,
                    listUser.TotalCount, listUser.TotalPages));
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

        [HttpPut]
        public async Task<ActionResult> UpdateUser(UserUpdateDto dto)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            if (user == null) return NotFound();

            _mapper.Map(dto, user);

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to Update user");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<UserPhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            if (user == null) return NotFound();

            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                UserId = user.Id,
                Partition = "Partition/" + user.Id
            };

            _photoRepository.Add(photo);
            await _photoRepository.SaveAllAsync();

            var photoDto = _mapper.Map<UserPhotoDto>(photo);

            if (user.Photos.Count == 0) photoDto.IsMain = true;

            user.Photos.Add(photoDto);

            if (await _userRepository.SaveAllAsync())
            {
                return CreatedAtAction(nameof(GetUser), new {username = user.Username}, photoDto);
            }

            return BadRequest("Problem adding photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(string photoId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            if (user == null) return NotFound();

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return NotFound();

            if (photo.IsMain) return BadRequest("this is already your main photo");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Problem setting the main photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(string photoId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return NotFound();

            if (photo.IsMain) return BadRequest("You cannot delete your main photo");

            if (photo.Id != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.Id);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            user.Photos.Remove(photo);

            return await _userRepository.SaveAllAsync() 
                ? Ok()
                : BadRequest("Problem deleting photo");
        }
    }
}
