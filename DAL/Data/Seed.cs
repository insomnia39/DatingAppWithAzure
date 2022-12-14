using DatingApp.DAL.Model;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using User = DatingApp.DAL.Model.User;

namespace DatingApp.DAL.Data
{
    public class Seed
    {
        public static async Task SeedUser(ProfileContext context)
        {
			try
			{
                if (context.User.ToList().Any()) return;

                var userData = await File.ReadAllTextAsync("..\\DAL\\Data\\UserSeed.json");

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var users = JsonSerializer.Deserialize<List<User>>(userData, options);

                foreach (var user in users)
                {
                    using var hmac = new HMACSHA512();
                    var rand = new Random();
                    user.Username = user.Username.ToLower();
                    user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("123123"));
                    user.PasswordSalt = hmac.Key;

                    context.User.Add(user);
                }

                await context.SaveChangesAsync();
            }
			catch (Exception e)
			{
				throw;
			}
        }

        public static async Task SeedPhoto(ProfileContext context)
        {
            try
            {
                if (context.Photo.ToList().Any()) return;

                var userData = context.User.ToList();

                foreach (var user in userData)
                {
                    foreach (var photo in user.Photos)
                    {
                        var newPhoto = new Photo();
                        newPhoto.Url = photo.Url;
                        newPhoto.IsMain = photo.IsMain;
                        newPhoto.UserId = user.Id;
                        newPhoto.Partition += "/" + newPhoto.UserId;
                        context.Photo.Add(newPhoto);
                    }
                }

                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }


}
