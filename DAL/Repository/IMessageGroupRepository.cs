using DatingApp.DAL.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatingApp.DAL.Repository
{
    public interface IMessageGroupRepository
    {
        Task CreateMessageGroup(MessageGroup msgGroup);
        Task SaveAllAsync();
        Task<MessageGroup> GetMessageGroupAsync(string groupId, string userId);
        Task<MessageGroup> GetUnreadMessageGroup(string groupId, string userId);
        Task<List<MessageGroup>> GetMessageGroupAsync(string userId);
    }
}
