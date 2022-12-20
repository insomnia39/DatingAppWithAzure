using DatingApp.BLL.MessageGroupManagement;
using DatingApp.DAL.DTO.Message;
using DatingApp.DAL.Model;
using DatingApp.DAL.UnitOfWork;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.BLL.MessageManagement
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMessageGroupService _msgGroupService;

        public MessageService(IUnitOfWork uow, IMessageGroupService msgGroupService)
        {
            _uow ??= uow;
            _msgGroupService ??= msgGroupService;
        }

        public async Task<Message> CreateMessageAsync(Message message)
        {
            if (message.SenderId == message.ReceiverId) 
                throw new ArgumentException("You can't send message to yourself");
            if (string.IsNullOrEmpty(message.GroupId))
            {
                var msgGroup = await _msgGroupService.CreateMessageGroupAsync(message);
                message.GroupId = msgGroup.GroupId;
            }else
            {
                await _msgGroupService.UpdateMessageGroupAsync(message);
            }

            message.Partition = "Partition/" + message.GroupId;
            _uow.MessageRepository.CreateMessage(message);

            await _uow.MessageRepository.SaveAllAsync();

            return message;
        }

        public async Task<List<Message>> GetMessageByGroupId(string groupId, string userId)
        {
            await SetReadAllMessage(groupId, userId);
            return await _uow.MessageRepository.GetMessageByGroupId(groupId);
        }

        public async Task DeleteMessage(DeleteMessageDto dto)
        {
            var message = await _uow.MessageRepository.GetMessage(dto.GroupId, dto.MessageId);
            if (message == null) throw new ArgumentException("Message is not available");

            var userAsSender = message.SenderId == dto.UserId;
            var userAsReceiver = message.ReceiverId == dto.UserId;

            if (!userAsSender && !userAsReceiver)
                throw new ArgumentException("You can't delete this message");
            if (userAsSender && message.SenderDelete)
                throw new ArgumentException("This message has been deleted");
            if (userAsReceiver && message.ReceiverDelete)
                throw new ArgumentException("This message has been deleted");
            if (userAsSender) message.SenderDelete = true;
            if (userAsReceiver) message.ReceiverDelete = true;
            if (message.ReceiverDelete && message.SenderDelete)
                _uow.MessageRepository.DeleteMessage(message);

            await _uow.MessageRepository.SaveAllAsync();
        }

        private async Task SetReadAllMessage(string groupId, string userId)
        {
            if (!await _msgGroupService.SetReadMessageGroup(groupId, userId)) return;
            var messages = await _uow.MessageRepository.GetUnreadMessage(groupId, userId);
            var readDate = DateTime.UtcNow;
            foreach (var message in messages)
            {
                message.Status = MessageProperty.Status.Read;
                message.ReadDate = readDate;
            }
            if (messages.Any()) await _uow.MessageRepository.SaveAllAsync();
        }

        private async Task ValidateMessageGroup(string groupId, string userId)
        {
            if (!await _msgGroupService.IsMessageGroupAvailable(groupId, userId))
                throw new ArgumentException("Message group is forbidden");
        }
    }
}
