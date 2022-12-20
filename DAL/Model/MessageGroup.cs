using System;

namespace DatingApp.DAL.Model
{
    public class MessageGroup : ModelBase
    {
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string LatestText { get; set; }
        public string LatestSenderId { get; set; }
        public DateTime LatestSent { get; set; } = DateTime.UtcNow;
        public int NumberOfUnreadMessage { get; set; }
        public string GroupId { get; set; }
        public string ReceiverId { get; set; }
    }
}
