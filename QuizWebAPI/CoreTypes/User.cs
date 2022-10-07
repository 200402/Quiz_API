namespace QuizWebAPI.CoreTypes
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
