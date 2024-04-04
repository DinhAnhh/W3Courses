using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLibrary.Models;

namespace WebLibrary.DAO
{
    public class EnrollmentDAO
    {
private static EnrollmentDAO instance = null;
        private static readonly object instanceLock = new object();
        public static EnrollmentDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new EnrollmentDAO();

                    }
                    return instance;
                }
            }
        }
         public IEnumerable<Enrollment> GetEnrollmentlist()
        {
            var Enrollment = new List<Enrollment>();
            try
            {
                using var context = new DBContext();
                Enrollment = context.Enrollments.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

                throw;
            }
            return Enrollment;
        }
        public Enrollment GetEnrollemnentByID(int learnerId, int courseId)
        {
            Learner learner = new Learner();
            Course course = new Course();
            Enrollment enrollment = null;
            var learnerID = learner.LearnerId;
            var courseID = course.CourseId;
            try
            {
                using var context = new DBContext();
                enrollment = context.Enrollments.SingleOrDefault(e => e.LearnerId == learnerId && e.CourseId == courseId);

            }
            catch (System.Exception)
            {

                throw;
            }
            return enrollment;
        }
        static bool checkEnroll(int learnerId, int courseId)
        {
            {
                List<Enrollment> en = new List<Enrollment>().ToList();
                bool hasEnrollment = en.Any(e => e.LearnerId == learnerId && e.CourseId == courseId);
                return hasEnrollment;
            }
        }
        public void AddNew(int learnerId, int courseId)
        {
            try
            {

                var l = LearnerDAO.Instance.GetLearnerByID(learnerId);
                var c = CourseDAO.Instance.GetCourseByID(courseId);
                Enrollment existingEnroll = GetEnrollemnentByID(learnerId, courseId);
                if (existingEnroll == null)
                {
                    using (var context = new DBContext())
                    {
                        Enrollment en = new Enrollment
                        {
                            LearnerId = learnerId,
                            CourseId = courseId,
                            CourseName = c.CourseName,
                            LearnerName =l.FirstName + l.LastName,
                            EnrollmentDate = DateTime.Now,
                            Email = l.Email,
                            IsPaid = true
                        };

                        context.Enrollments.Add(en);
                        context.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception("The Enrollemnt already exists.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
       
    }
}