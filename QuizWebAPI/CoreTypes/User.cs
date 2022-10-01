namespace QuizWebAPI.CoreTypes
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;
    }
}
