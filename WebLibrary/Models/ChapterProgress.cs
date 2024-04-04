using System;
using System.Collections.Generic;

#nullable disable

namespace WebLibrary.Models
{
    public partial class ChapterProgress
    {
        public int ChapterProgressId { get; set; }
        public int? ChapterId { get; set; }
        public int? CourseProgressId { get; set; }
        public int? ProgressPercent { get; set; }
        public bool? Completed { get; set; }
        public int? TotalTime { get; set; }
        public DateTime? StartAt { get; set; }

        public virtual Chapter Chapter { get; set; }
        public virtual CourseProgress CourseProgress { get; set; }
    }
}
