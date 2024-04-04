using System;
using System.Collections.Generic;

#nullable disable

namespace WebLibrary.Models
{
    public partial class Enrollment
    {
        public int EnrollmentId { get; set; }
        public int LearnerId { get; set; }
        public int CourseId { get; set; }
        public string LearnerName { get; set; }
        public string CourseName { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string Email { get; set; }
        public bool IsPaid { get; set; }

        public virtual Course Course { get; set; }
        public virtual Learner Learner { get; set; }
    }
}
