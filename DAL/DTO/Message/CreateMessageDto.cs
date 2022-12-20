namespace DatingApp.DAL.DTO.Message
{
    public class CreateMessageDto
    {
        public string ReceiverId { get; set; }
        public string Text { get; set; }
        public string GroupId { get; set; }
    }
}
