using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.BLL.Helpers;
using DatingApp.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace DatingApp.BLL.Photo
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;
        private readonly ProfileContext _context;

        public PhotoService(IOptions<CloudinarySettings> config, ProfileContext context)
        {
            var acc = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);
            _cloudinary = new Cloudinary(acc);
            _context ??= context;
        }

        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
                    Folder = "da-net7"
                };

                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }

            return uploadResult;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string id)
        {
            var photo = await _context.Photo.FindAsync(id);

            if (photo == null) throw new Exception("photo not found");

            var deleteParams = new DeletionParams(photo.PublicId);

            var result = await _cloudinary.DestroyAsync(deleteParams);

            if (result.Error == null)
            {
                _context.Photo.Remove(photo);
                await _context.SaveChangesAsync();
            }

            return result;
        }
    }
}
