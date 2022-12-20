using DatingApp.DAL.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatingApp.BLL.MessageGroupManagement
{
    public interface IMessageGroupService
    {
        Task<bool> IsMessageGroupAvailable(string groupId, string userId);
        Task<MessageGroup> CreateMessageGroupAsync(Message message);
        Task<MessageGroup> GetMessageGroupAsync(string groupId, string userId);
        Task UpdateMessageGroupAsync(Message message);
        Task<bool> SetReadMessageGroup(string groupId, string userId);
        Task<List<MessageGroup>> GetMessageGroupAsync(string userId);
    }
}
