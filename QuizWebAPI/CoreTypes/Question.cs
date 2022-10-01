using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizWebAPI.CoreTypes
{
    internal class Question
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public string? ImageURL { get; set; } //или обжект, по хоту дела определюсь
        public int IdTopic { get; set; }
        public int? QuestionLVL { get; set; }
    }
}
