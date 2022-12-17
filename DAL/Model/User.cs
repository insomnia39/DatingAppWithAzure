using DatingApp.BLL.Extensions;
using DatingApp.DAL.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DatingApp.DAL.Model
{
    public static class UserProperty
    {
        public static class Gender
        {
            public const string Male = nameof(Male);
            public const string Female = nameof(Female);
        }
    }

    public class User : ModelBase
    {
        public string Username { get; set; }
        public string Gender { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public List<UserPhotoDto> Photos { get; set; } = new();

        public int GetAge()
        {
            return DateOfBirth.CalculateAge();
        }

        public string GetPhotoUrl()
        {
            var url = Photos.FirstOrDefault(p => p.IsMain)?.Url;
            return url;
        }
    }
}
