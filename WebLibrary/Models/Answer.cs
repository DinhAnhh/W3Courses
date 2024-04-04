using System;
using System.Collections.Generic;

#nullable disable

namespace WebLibrary.Models
{
    public partial class Answer
    {
        public Answer()
        {
            Quizzes = new HashSet<Quiz>();
        }

        public int AnswerId { get; set; }
        public string Answer1 { get; set; }

        public virtual ICollection<Quiz> Quizzes { get; set; }
    }
}
