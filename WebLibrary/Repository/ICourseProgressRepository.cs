using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLibrary.Models;

namespace WebLibrary.Repository
{
    public interface ICourseProgressRepository
    {
         IEnumerable<CourseProgress> GetCourseProgresss();
        CourseProgress GetCourseProgressByID(int CourseProgressId);
        void InsertCourseProgress(CourseProgress CourseProgress);
        void DeleteCourseProgress(int CourseProgressId);
        void UpdateCourseProgress(CourseProgress CourseProgress);
        CourseProgress GetCourseProgressByLearnerAndCourse(int learnerId, int courseId);
    } 
}