using System.Text.Json.Serialization;

namespace DatingApp.DAL.DTO.Account
{
    public class UserRequestDto
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }
        [JsonPropertyName("gender")]
        public string Gender { get; set; }
    }

    public class UserResponseDto
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
