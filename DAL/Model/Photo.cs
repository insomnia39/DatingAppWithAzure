namespace DatingApp.DAL.Model
{
    public class Photo : ModelBase
    {
        public string Url { get; set; }
        public string UserId { get; set; }
        public string PublicId { get; set; }
    }
}