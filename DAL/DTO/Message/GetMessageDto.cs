using DatingApp.DAL.Model;
using System;

namespace DatingApp.DAL.DTO.Message
{
    public class GetMessageDto
    {
        public string Id { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Text { get; set; }
        public DateTime SentDate { get; set; } = DateTime.UtcNow;
        public DateTime ReadDate { get; set; } = DateTime.MinValue;
        public string Status { get; set; } = MessageProperty.Status.Sent;
        public string GroupId { get; set; }
        public bool? SenderDelete { get; set; } = false;
        public bool? ReceiverDelete { get; set; } = false;
    }
}
