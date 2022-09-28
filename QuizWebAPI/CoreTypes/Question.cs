using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizWebAPI.CoreTypes
{
    internal class Question
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string QuestionText { get; set; }
        public string? ImageURL { get; set; } //или обжект, по хоту дела определюсь
        [Required]
        public int IdTopic { get; set; }
        public int? QuestionLVL { get; set; }
    }
}
