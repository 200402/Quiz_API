namespace QuizWebAPI.CoreTypes
{
    public class User
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int Login { get; set; }

        [Required]
        [StringLength(64, MinimumLength = 4)]
        public string Password { get; set; }

        public string Token { get; set; } = string.Empty;
    }
}
