using DatingApp.DAL;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.BLL.Repository
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly ProfileContext _context;

        public PhotoRepository(ProfileContext context)
        {
            _context ??= context;
        }

        public void Add(DAL.Model.Photo photo)
        {
            _context.Photo.Add(photo);
        }

        public void DeleteByPublicId(string publicId, string userId)
        {
            var photo = _context.Photo.WithPartitionKey(userId).FirstOrDefault(p => p.PublicId == publicId);
            _context.Photo.Remove(photo);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
