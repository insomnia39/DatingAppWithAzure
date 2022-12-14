using AutoMapper;
using DatingApp.DAL.DTO.User;
using DatingApp.DAL.Model;

namespace DatingApp.BLL.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserDto>();
        }
    }
}
