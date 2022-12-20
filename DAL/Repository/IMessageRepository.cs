using DatingApp.DAL.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatingApp.DAL.Repository
{
    public interface IMessageRepository
    {
        void CreateMessage(Message message);
        void DeleteMessage(Message message);
        Task SaveAllAsync();
        Task<List<Message>> GetMessageByGroupId(string groupId);
        Task<List<Message>> GetUnreadMessage(string groupId, string userId);
        Task<Message> GetMessage(string groupId, string chatId);
    }
}
