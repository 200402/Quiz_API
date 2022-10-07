namespace QuizWebAPI.Data
{
    public class UserRepository
    {
        //private QuizDb Db;
        private SqlConnection myConn;
        private SqlCommand myCommand;


        public UserRepository(SqlConnection myConn)
        {
            this.myConn = myConn;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            List<User>userList = new();
            string sql = "SELECT * FROM Users";
            using (myCommand = new SqlCommand(sql, myConn))
            {
                myConn.Open();
                await using (SqlDataReader reader = myCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        userList.Add(readerToUser(reader));
                    }
                }
                myConn.Close();
            }
            return userList;
        }

        public async Task<User> GetUserByIdAsync(int Id)
        {
            User user = new();
            string sql = $"SELECT * FROM Users WHERE  Id = '{Id}'";
            using (myCommand = new SqlCommand(sql, myConn))
            {
                myConn.Open();
                await using (SqlDataReader reader = myCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        user = readerToUser(reader);
                    }
                }
                myConn.Close();
            }
            return user;
        }

        public async Task<User> GetUserByTokenAsync(string token)
        {
            User user = new();
            string sql = $"SELECT * FROM Users WHERE  Token = '{token}'";
            using (myCommand = new SqlCommand(sql, myConn))
            {
                myConn.Open();
                await using (SqlDataReader reader = myCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        user = readerToUser(reader);
                    }
                }
                myConn.Close();
            }
            return user;
        }

        public async Task<User> GetUserByLoginAsync(string Login)
        {
            User user = new();
            string sql = $"SELECT * FROM Users WHERE Login = '{Login}'";
            using (myCommand = new SqlCommand(sql, myConn))
            {
                myConn.Open();
                await using (SqlDataReader reader = myCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        user = readerToUser(reader);
                    }
                }
                myConn.Close();
            }
            return user;
        }

        public async Task<User> CreateRandomUser()
        {
            User user = new();
            string sql = "SELECT COUNT(*) FROM Users";
            int count = 0;
            using (myCommand = new SqlCommand(sql, myConn))
            {
                myConn.Open();
                await using (SqlDataReader reader = myCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        count = Convert.ToInt32(reader[0]);
                    }
                }
                myConn.Close();
            }
            if (count < 100)
            {
                user.Login = "TEST" + GenerateRandomString(5, 20);
                user.Name = "TEST" + GenerateRandomString(5, 20);
                user.Password = "TEST" + GenerateRandomString(5, 20);
                user.ImageUrl = "TEST" + GenerateRandomString(5, 20);
                user.Token = "TEST" + GenerateRandomString(50, 100);
                sql = "INSERT INTO Users (Login, Name, Password, ImageUrl, Token)" +
                    $"VALUES ('{user.Login}', '{user.Name}', '{user.Password}', '{user.ImageUrl}', '{user.Token}')";
                using (myCommand = new SqlCommand(sql, myConn))
                {
                    myConn.Open();
                    myCommand.ExecuteNonQuery();
                    myConn.Close();
                }
                return user;
            }
            return null;
        }

    private User readerToUser(SqlDataReader reader)
        {
            User user = new User();
            user.Id = Convert.ToInt32(reader[0]);
            user.Login = reader[1].ToString();
            user.Password = reader[2].ToString();
            user.Token = reader[3].ToString();
            user.Name = reader[4].ToString();
            user.ImageUrl = reader[5].ToString();
            return user;
        }

        public async Task<User> InsertUserAsync(NewUser newUser)
        {
            User user = new();
            user.Login = newUser.Login;
            user.Password = newUser.Password;
            user.Name = newUser.Name;
            user.Token = GenerateRandomString(50, 100);
            var sql = "INSERT INTO Users (Login, Name, Password, ImageUrl, Token)" +
                $"VALUES ('{user.Login}', '{user.Name}', '{user.Password}', '{user.ImageUrl}', '{user.Token}')";
            using (myCommand = new SqlCommand(sql, myConn))
            {
                myConn.Open();
                myCommand.ExecuteNonQuery();
                myConn.Close();
            }
            return user;
        }

        //public async Task UpdateUserAsync(User user)
        //{
        //    var userFromDb = await Db.Users.FindAsync(new object[] { user.Id, user.Token });
        //    if (userFromDb == null) 
        //        return;
        //    userFromDb.Login = user.Login;
        //    userFromDb.Password = user.Password;
        //    userFromDb.Token = user.Token;
        //}

        //public async Task DeleteUserAsync(int Id)
        //{
        //    var userFromDb = await Db.Users.FindAsync(new object[] { Id });
        //    if (userFromDb == null) return;
        //    Db.Users.Remove(userFromDb);
        //}

        //public async Task SaveAsync() => await Db.SaveChangesAsync();
        //public void Save() => Db.SaveChanges();

        //private bool _disposed = false;
        //protected virtual void Dispose(bool disposing)
        //{
        //    if (!_disposed) { if (disposing) Db.Dispose(); }
        //    _disposed = true;
        //}

        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}

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
