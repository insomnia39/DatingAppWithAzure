using System;

namespace DatingApp.DAL.Model
{
    public static class MessageProperty
    {
        public static class Status
        {
            public const string Sent = nameof(Sent);
            public const string Read = nameof(Read);
        }
    }

    public class Message : ModelBase
    {
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Text { get; set; }
        public DateTime SentDate { get; set; } = DateTime.UtcNow;
        public DateTime ReadDate { get; set; } = DateTime.MinValue;
        public string Status { get; set; } = MessageProperty.Status.Sent;
        public string GroupId { get; set; }
        public bool SenderDelete { get; set; } = false;
        public bool ReceiverDelete { get; set; } = false;
    }
}
