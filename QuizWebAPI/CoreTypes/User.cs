using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizWebAPI.CoreTypes
{
    public class User
    {
        public int Id { get; set; }
        public int Login { get; set; }
        public string Password { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
