using System;
using System.Collections.Generic;

#nullable disable

namespace WebLibrary.Models
{
    public partial class Course
    {
        public Course()
        {
            Chapters = new HashSet<Chapter>();
            CourseProgresses = new HashSet<CourseProgress>();
            Enrollments = new HashSet<Enrollment>();
            Instructs = new HashSet<Instruct>();
            Lessons = new HashSet<Lesson>();
            Quizzes = new HashSet<Quiz>();
            Reviews = new HashSet<Review>();
        }

        public int CourseId { get; set; }
        public int? CategoryId { get; set; }
        public string CourseName { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public int? TotalTime { get; set; }
        public DateTime? CreationDate { get; set; }
        public decimal? Price { get; set; }
        public string Status { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<Chapter> Chapters { get; set; }
        public virtual ICollection<CourseProgress> CourseProgresses { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual ICollection<Instruct> Instructs { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
        public virtual ICollection<Quiz> Quizzes { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
