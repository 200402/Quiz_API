using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizWebAPI.CoreTypes
{
    internal class CorrecttAnswer
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int IdQuestion { get; set; }
        [Required]
        public int IdAnswer { get; set; }
    }
}
