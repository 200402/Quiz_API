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

        public async Task<User> AuthorizationUser(string login, string password)
        {
            var user = await Db.Users.FindAsync(new object[] { login, password });
            if (user == null)
                return null;
            user.Token = GenerateRandomString(50);
            await UpdateUserAsync(user);
            user.Login = "";
            user.Password = "";
            return user;
        }

        public async Task<User> CreateRandomUser() 
        {
            if (Db.Users.Count() < 100)
            {
                var user = new User();
                user.Login = GenerateRandomString(5,10);
                user.Password = GenerateRandomString(5, 10);
                await InsertUserAsync(user);
                return user;
            }
            else
            {
                return new User { Password = "ПОШОЛ", Login = "НАХУЙ" };
            }
        }

        public async Task<User> DeleteAll() 
        {
            foreach (var row in Db.Users)
            {
                 Db.Users.Remove(row);
            }
            return new User();
        }

        public async Task InsertUserAsync(User user)
        {
            var userFromDb = await Db.Users.FindAsync(new object[] { user.Login });
            if (userFromDb == null)
                await Db.Users.AddAsync(user);
            return; 
        }

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

        private string GenerateRandomString(int lenght)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            chars += chars.ToLower() + "0123456789";
            var r = new Random();
            return new string(Enumerable.Repeat(chars, lenght)
                    .Select(s => s[r.Next(s.Length)]).ToArray());
        }

        private string GenerateRandomString(int minLenght, int maxLenght)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            chars += chars.ToLower() + "0123456789";
            var r = new Random();
            return new string(Enumerable.Repeat(chars, r.Next(minLenght,maxLenght))
                    .Select(s => s[r.Next(s.Length)]).ToArray());
        }
    }
}
