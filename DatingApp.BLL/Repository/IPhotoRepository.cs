using DatingApp.DAL.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatingApp.BLL.Repository
{
    public interface IPhotoRepository
    {
        void Add(DAL.Model.Photo photo);
        Task<bool> SaveAllAsync();
        void DeleteByPublicId(string publicId, string userId);
    }
}
