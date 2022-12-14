using DatingApp.BLL.Extensions;
using System;
using System.Collections.Generic; 

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
        public List<UserPhoto> Photos { get; set; } = new();

        public int GetAge()
        {
            return DateOfBirth.CalculateAge();
        }
    }

    public class UserPhoto
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
    }
}
