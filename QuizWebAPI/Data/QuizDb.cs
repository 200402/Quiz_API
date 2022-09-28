namespace QuizWebAPI.Data
{
    public class QuizDb : DbContext
    {
        public QuizDb(DbContextOptions<QuizDb> options) : base(options) { }
        public DbSet<User> Users => Set<User>();
    }
}
