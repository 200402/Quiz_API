using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizWebAPI.CoreTypes
{
    internal class Answer
    {
        [Required]
        public int ID { get; set; }
        [Required]
        public string AnswerText { get; set; }
        [Required]
        public int IDTopic { get; set; }
    }
}
