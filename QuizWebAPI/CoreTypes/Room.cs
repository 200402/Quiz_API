using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizWebAPI.CoreTypes
{
    internal class Room
    { 
        public int Id { get; set; }
        public string Status { get; set; } // возможно нужно будет отдельный класс для статусов
        public int[] IdUsers { get; set; }
        public int[] UsersScore { get; set; } // поменяет по мере разработки идеи
    }
}
