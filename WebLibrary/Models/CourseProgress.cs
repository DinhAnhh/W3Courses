using System;
using System.Collections.Generic;

#nullable disable

namespace WebLibrary.Models
{
    public partial class CourseProgress
    {
        public CourseProgress()
        {
            ChapterProgresses = new HashSet<ChapterProgress>();
        }

        public int CourseProgressId { get; set; }
        public int? LearnerId { get; set; }
        public int? CourseId { get; set; }
        public bool? Completed { get; set; }
        public int? ProgressPercent { get; set; }
        public bool? Rated { get; set; }
        public int? TotalTime { get; set; }
        public DateTime? StartAt { get; set; }

        public virtual Course Course { get; set; }
        public virtual Learner Learner { get; set; }
        public virtual ICollection<ChapterProgress> ChapterProgresses { get; set; }
    }
}
