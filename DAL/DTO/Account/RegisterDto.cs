using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DatingApp.DAL.DTO.Account
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string KnownAs { get; set; }
        [Required]
        public DateTime? DateOfBirth { get; set; }
        [Required]
        public string City { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 4)]
        public string Password { get; set; }
    }
}
