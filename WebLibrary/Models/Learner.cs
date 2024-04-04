using System;
using System.Collections.Generic;

#nullable disable

namespace WebLibrary.Models
{
    public partial class Learner
    {
        public Learner()
        {
            CourseProgresses = new HashSet<CourseProgress>();
            Enrollments = new HashSet<Enrollment>();
            LessonProgresses = new HashSet<LessonProgress>();
            Reviews = new HashSet<Review>();
            VoucherUsages = new HashSet<VoucherUsage>();
        }

        public int LearnerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Picture { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string Status { get; set; }

        public virtual ICollection<CourseProgress> CourseProgresses { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual ICollection<LessonProgress> LessonProgresses { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<VoucherUsage> VoucherUsages { get; set; }
    }
}
