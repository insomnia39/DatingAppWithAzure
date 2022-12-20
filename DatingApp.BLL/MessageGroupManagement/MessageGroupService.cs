using DatingApp.DAL.Model;
using DatingApp.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatingApp.BLL.MessageGroupManagement
{
    public class MessageGroupService : IMessageGroupService
    {
        private readonly IUnitOfWork _uow;

        public MessageGroupService(IUnitOfWork uow)
        {
            _uow ??= uow;
        }

        public async Task<bool> IsMessageGroupAvailable(string groupId, string userId)
        {
            var msgGroup = await GetMessageGroupAsync(groupId, userId);
            return msgGroup == null;
        }

        public async Task<MessageGroup> CreateMessageGroupAsync(Message message)
        {
            try
            {
                var groupId = Guid.NewGuid().ToString();
                var senderMessageGroup = new MessageGroup
                {
                    GroupId = groupId,
                    LatestText = message.Text,
                    LatestSent = message.SentDate,
                    LatestSenderId = message.SenderId,
                    Partition = "Partition/" + message.SenderId,
                    ReceiverId = message.ReceiverId
                };

                var receiverMessageGroup = new MessageGroup
                {
                    GroupId = groupId,
                    CreatedDate = senderMessageGroup.CreatedDate,
                    LatestText = message.Text,
                    LatestSent = message.SentDate,
                    LatestSenderId = message.SenderId,
                    Partition = "Partition/" + message.ReceiverId,
                    NumberOfUnreadMessage = 1,
                    ReceiverId = message.SenderId
                };

                await _uow.MessageGroupRepository.CreateMessageGroup(senderMessageGroup);
                await _uow.MessageGroupRepository.CreateMessageGroup(receiverMessageGroup);
                return senderMessageGroup;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<MessageGroup> GetMessageGroupAsync(string groupId, string userId)
        {
            return await _uow.MessageGroupRepository.GetMessageGroupAsync(groupId, userId);
        }

        public async Task UpdateMessageGroupAsync(Message message)
        {
            var msgGroupSender = await GetMessageGroupAsync(message.GroupId, message.SenderId);
            msgGroupSender.LatestText = message.Text;
            msgGroupSender.LatestSent = message.SentDate;
            msgGroupSender.LatestSenderId = message.SenderId;
            var msgGroupReceiver = await GetMessageGroupAsync(message.GroupId, message.ReceiverId);
            msgGroupReceiver.LatestText = message.Text;
            msgGroupReceiver.LatestSent = message.SentDate;
            msgGroupReceiver.LatestSenderId = message.SenderId;
            msgGroupReceiver.NumberOfUnreadMessage += 1;

            await _uow.MessageGroupRepository.SaveAllAsync();
        }

        public async Task<bool> SetReadMessageGroup(string groupId, string userId)
        {
            var unreadMsgGroup = await _uow.MessageGroupRepository.GetUnreadMessageGroup(groupId, userId);
            if (unreadMsgGroup == null) return false;
            unreadMsgGroup.NumberOfUnreadMessage = 0;
            await _uow.MessageGroupRepository.SaveAllAsync();
            return true;
        }

        public async Task<List<MessageGroup>> GetMessageGroupAsync(string userId)
        {
            return await _uow.MessageGroupRepository.GetMessageGroupAsync(userId);
        }
    }
}
