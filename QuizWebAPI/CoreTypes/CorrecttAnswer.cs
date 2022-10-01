using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizWebAPI.CoreTypes
{
    internal class CorrecttAnswer
    {
        public int Id { get; set; }
        public int IdQuestion { get; set; }
        public int IdAnswer { get; set; }
    }
}
