namespace QuizWebAPI.Data
{
    public class UserRepository
    {
        private QuizDb Db;

        public UserRepository(QuizDb db)
        {
            Db = db;
        }

        public async Task<List<User>> GetAllUsersAsync() =>
            await Db.Users.ToListAsync();

        public async Task<User> GetUserByIdAsync(int Id) =>
            await Db.Users.FindAsync(new object[] { Id });

        public async Task<User> CreateRandomUser() 
        {
            if (Db.Users.Count() < 100)
            {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var user = new User();
                var r = new Random();
                user.Login = new string(Enumerable.Repeat(chars, r.Next(5, 10))
                        .Select(s => s[r.Next(s.Length)]).ToArray());
                user.Password = new string(Enumerable.Repeat(chars, r.Next(5, 10))
                        .Select(s => s[r.Next(s.Length)]).ToArray());
                await InsertUserAsync(user);
                return user;
            }
            else
            {
                return new User { Password = "ПОШОЛ", Login = "НАХУЙ" };
            }
        }

        public async Task InsertUserAsync(User user) => await Db.Users.AddAsync(user);

        public async Task UpdateUserAsync(User user)
        {
            var userFromDb = await Db.Users.FindAsync(new object[] { user.Id });
            if (userFromDb == null) 
                return;
            userFromDb.Login = user.Login;
            userFromDb.Password = user.Password;
            userFromDb.Token = user.Token;
        }

        public async Task DeleteUserAsync(int Id)
        {
            var userFromDb = await Db.Users.FindAsync(new object[] { Id });
            if (userFromDb == null) return;
            Db.Users.Remove(userFromDb);
        }

        public async Task SaveAsync() => await Db.SaveChangesAsync();

        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed) { if (disposing) Db.Dispose(); }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
