using AutoMapper;
using DatingApp.DAL.DTO.Account;
using DatingApp.DAL.DTO.User;
using DatingApp.DAL.Model;

namespace DatingApp.BLL.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(p => p.Id));
            CreateMap<UserUpdateDto, User>();
            CreateMap<DAL.Model.Photo, UserPhotoDto>();
            CreateMap<RegisterDto, User>();
            CreateMap<User, UserLoginDto>();
        } 
    }
}
