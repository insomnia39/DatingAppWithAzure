using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DatingApp.DAL.DTO.Account
{
    public class RegisterDto
    {
        [Required]
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [Required]
        [JsonPropertyName("gender")]
        public string Gender { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 4)]
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
