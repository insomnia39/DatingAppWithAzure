using System;

namespace DatingApp.DAL.DTO.MessageGroup
{
    public class GetMessageGroupDto
    {
        public string Id { get; set; }
        public string LatestText { get; set; }
        public string LatestSenderId { get; set; }
        public DateTime LatestSent { get; set; }
        public int NumberOfUnreadMessage { get; set; }
        public string GroupId { get; set; }
        public string ReceiverId { get; set; }
        public string ReceiverUsername { get; set; }
        public string ReceiverPhotoUrl { get; set; }
    }
}
