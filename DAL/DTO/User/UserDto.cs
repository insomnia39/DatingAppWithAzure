using System.Text.Json.Serialization;

namespace DatingApp.DAL.DTO.User
{
    public class UserDto
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}
