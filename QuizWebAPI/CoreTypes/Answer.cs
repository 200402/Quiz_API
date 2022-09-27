using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizWebAPI.CoreTypes
{
    internal class Answer
    {
        public int ID { get; set; }
        public string AnswerText { get; set; }
        public int IDTopic { get; set; }
    }
}
