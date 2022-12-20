using DatingApp.DAL.Helper;
using DatingApp.DAL.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.DAL.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ProfileContext _context;

        public MessageRepository(ProfileContext context)
        {
            _context ??= context;
        }

        public void CreateMessage(Message message)
        {
            _context.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _context.Remove(message);
        }

        public async Task<List<Message>> GetMessageByGroupId(string groupId)
        {
            string partition = GetPartition(groupId);
            var messages = await _context.Message.WithPartitionKey(partition).ToListAsync();
            return messages;
        }

        public async Task<List<Message>> GetUnreadMessage(string groupId, string userId)
        {
            string partition = GetPartition(groupId);
            var query = _context.Message.WithPartitionKey(partition).AsQueryable();
            query = query.Where(p => p.Status == MessageProperty.Status.Sent);
            query = query.Where(p => p.ReceiverId == userId);
            return await query.ToListAsync();
        }

        public async Task<Message> GetMessage(string groupId, string chatId)
        {
            string partition = GetPartition(groupId);
            var query = _context.Message.WithPartitionKey(partition).AsQueryable();
            query = query.Where(p => p.Id == chatId);
            return await query.FirstOrDefaultAsync();
        }

        public async Task SaveAllAsync()
        {
            await _context.SaveChangesAsync();
        }

        private string GetPartition(string groupId)
        {
            return "Partition/" + groupId;
        }
    }
}
