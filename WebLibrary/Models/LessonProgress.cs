using System;
using System.Collections.Generic;

#nullable disable

namespace WebLibrary.Models
{
    public partial class LessonProgress
    {
        public int LessonProgressId { get; set; }
        public int? LearnerId { get; set; }
        public int? LessonId { get; set; }
        public int? ChapterId { get; set; }
        public bool? Completed { get; set; }
        public DateTime? StartAt { get; set; }

        public virtual Learner Learner { get; set; }
        public virtual Lesson Lesson { get; set; }
    }
}
