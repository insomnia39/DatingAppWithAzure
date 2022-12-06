namespace DatingApp.DAL.Model
{
    public static class UserProperty
    {
        public static class Gender
        {
            public const string Male = nameof(Male);
            public const string Female = nameof(Female);
        }
    }

    public class User : ModelBase
    {
        public string Username { get; set; }
        public string Gender { get; set; }
    }
}
