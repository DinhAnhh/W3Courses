using System;
using System.Collections.Generic;

#nullable disable

namespace WebLibrary.Models
{
    public partial class Quiz
    {
        public int QuizId { get; set; }
        public int? CourseId { get; set; }
        public int? ChapterId { get; set; }
        public int? AnswerId { get; set; }
        public string Quiz1 { get; set; }
        public string DapAnA { get; set; }
        public string DapAnB { get; set; }
        public string DapAnC { get; set; }
        public string DapAnD { get; set; }
        public string Note { get; set; }
        public bool? IsCorrect { get; set; }

        public virtual Answer Answer { get; set; }
        public virtual Chapter Chapter { get; set; }
        public virtual Course Course { get; set; }
    }
}
