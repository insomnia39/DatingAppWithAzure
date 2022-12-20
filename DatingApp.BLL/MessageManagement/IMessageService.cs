using DatingApp.DAL.DTO.Message;
using DatingApp.DAL.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatingApp.BLL.MessageManagement
{
    public interface IMessageService
    {
        Task<Message> CreateMessageAsync(Message message);
        Task<List<Message>> GetMessageByGroupId(string groupId, string userId);
        Task DeleteMessage(DeleteMessageDto dto);
    }
}
