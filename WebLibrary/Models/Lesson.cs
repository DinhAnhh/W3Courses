using System;
using System.Collections.Generic;

#nullable disable

namespace WebLibrary.Models
{
    public partial class Lesson
    {
        public Lesson()
        {
            LessonProgresses = new HashSet<LessonProgress>();
        }

        public int LessonId { get; set; }
        public int? CourseId { get; set; }
        public int? ChapterId { get; set; }
        public string LessonName { get; set; }
        public string Description { get; set; }
        public int? PercentToPassed { get; set; }
        public string Content { get; set; }
        public int? Index { get; set; }
        public int? Time { get; set; }

        public virtual Chapter Chapter { get; set; }
        public virtual Course Course { get; set; }
        public virtual ICollection<LessonProgress> LessonProgresses { get; set; }
    }
}
