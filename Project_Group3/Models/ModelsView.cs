using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLibrary.Models;

namespace Project_Group3.Models
{
    public class ModelsView
    {
        public Learner Learner { get; set; }
        public Instructor Instructor { get; set; }
        public Course Course { get; set; }
        public Instruct Instruct { get; set; }
        public Chapter Chapter { get; set; }
        public Lesson Lesson { get; set; }
        public Quiz Quiz { get; set; }
        public Answer Answer { get; set; }
        public IList<Enrollment> EnrollmentList { get; set; } = new List<Enrollment>();
        public IList<Course> CourseList { get; set; } = new List<Course>();
        public IList<Category> CategoriesList { get; set; } = new List<Category>();
        public IList<Instruct> InstructsList { get; set; } = new List<Instruct>();
        public IList<Instructor> InstructorsList { get; set; } = new List<Instructor>();
        public IList<Review> ReviewsList { get; set; } = new List<Review>();
        public IList<Learner> LearnersList { get; set; } = new List<Learner>();
        public IList<Chapter> ChaptersList { get; set; } = new List<Chapter>();
        public IList<Lesson> LessonsList { get; set; } = new List<Lesson>();
        public IList<Admin> AdminsList { get; set; } = new List<Admin>();
    }
}

