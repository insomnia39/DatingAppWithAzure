using System.Text.Json.Serialization;

namespace DatingApp.DAL.DTO.User
{
    public class UserLoginDto
    {
        public string Username { get; set; }

        public string Token { get; set; }

        public string PhotoUrl { get; set; }
    }
}
