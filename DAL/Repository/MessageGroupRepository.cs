using DatingApp.DAL.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.DAL.Repository
{
    public class MessageGroupRepository : IMessageGroupRepository
    {
        private readonly ProfileContext _context;
        public MessageGroupRepository(ProfileContext context)
        {
            _context ??= context;
        }

        public async Task CreateMessageGroup(MessageGroup msgGroup)
        {
            _context.Add(msgGroup);
            await _context.SaveChangesAsync();
            _context.Entry(msgGroup).State = EntityState.Detached;
        }

        public async Task SaveAllAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<MessageGroup> GetMessageGroupAsync(string groupId, string userId)
        {
            string partition = GetPartition(userId);
            var query = _context.MessageGroup.AsQueryable();
            query = query.WithPartitionKey(partition);
            query = query.Where(p => p.GroupId == groupId);
            var msgGroup = await query.SingleOrDefaultAsync();
            return msgGroup;
        }

        public async Task<MessageGroup> GetUnreadMessageGroup(string groupId, string userId)
        {
            string partition = GetPartition(userId);
            var query = _context.MessageGroup.AsQueryable();
            query = query.WithPartitionKey(partition);
            query = query.Where(p => p.GroupId == groupId);
            query = query.Where(p => p.NumberOfUnreadMessage > 0);
            return await query.SingleOrDefaultAsync();
        }

        public async Task<List<MessageGroup>> GetMessageGroupAsync(string userId)
        {
            string partition = GetPartition(userId);
            var query = _context.MessageGroup.AsQueryable();
            query = query.WithPartitionKey(partition);
            query = query.OrderByDescending(p => p.LatestSent);
            return await query.ToListAsync();
        }

        private static string GetPartition(string userId)
        {
            return "Partition/" + userId;
        }
    }
}
