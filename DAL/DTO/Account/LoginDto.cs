using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DatingApp.DAL.DTO.Account
{
    public class LoginRequestDto
    {
        [Required]
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [Required]
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }

    public class LoginResponseDto
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("gender")]
        public string Gender { get; set; }
    }
}
