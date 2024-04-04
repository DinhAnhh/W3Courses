using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLibrary.DAO;
using WebLibrary.Models;

namespace WebLibrary.Repository
{
    public class CourseProgressRepository : ICourseProgressRepository
    {
           public CourseProgress GetCourseProgressByID(int CourseProgressId) => CourseProgressDAO.Instance.GetCourseProgressByID(CourseProgressId);
        public IEnumerable<CourseProgress> GetCourseProgresss() => CourseProgressDAO.Instance.GetCourseProgresslist();
        public void InsertCourseProgress(CourseProgress CourseProgress) => CourseProgressDAO.Instance.AddNew(CourseProgress);
        public void DeleteCourseProgress(int CourseProgressId) => CourseProgressDAO.Instance.Remove(CourseProgressId);
        public void UpdateCourseProgress(CourseProgress CourseProgress) => CourseProgressDAO.Instance.Update(CourseProgress);
        public CourseProgress GetCourseProgressByLearnerAndCourse(int learnerId, int courseId) => CourseProgressDAO.Instance.GetCourseProgressByLearnerAndCourse(learnerId, courseId);
    }
}