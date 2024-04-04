using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLibrary.Models;

namespace WebLibrary.DAO
{
    public class CourseProgressDAO
    {
        private static CourseProgressDAO instance = null;
        private static readonly object instanceLock = new object();
        public static CourseProgressDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CourseProgressDAO();

                    }
                    return instance;
                }
            }
        }
        public IEnumerable<CourseProgress> GetCourseProgresslist()
        {
            var CourseProgress = new List<CourseProgress>();
            try
            {
                using var context = new DBContext();
                CourseProgress = context.CourseProgresses.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

                throw;
            }
            return CourseProgress;
        }

        public CourseProgress GetCourseProgressByID(int CourseProgressID)
        {
            CourseProgress CourseProgress = null;
            try
            { 
                using var context = new DBContext();
                CourseProgress = context.CourseProgresses.SingleOrDefault(c => c.CourseProgressId.Equals(CourseProgressID));

            }
            catch (System.Exception)
            {

                throw;
            }
            return CourseProgress;
        }

        public void AddNew(CourseProgress CourseProgress)
        {
            try
            {
                CourseProgress existingCourseProgress = GetCourseProgressByID(CourseProgress.CourseProgressId);
                if (existingCourseProgress == null)
                {
                    using (var context = new DBContext())
                    {
                        context.CourseProgresses.Add(CourseProgress);
                        context.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception("The CourseProgress already exists.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Update(CourseProgress CourseProgress)
        {
            try
            {
                CourseProgress existingCourseProgress = GetCourseProgressByID(CourseProgress.CourseProgressId);
                if (existingCourseProgress != null)
                {
                    using (var context = new DBContext())
                    {
                        context.CourseProgresses.Update(CourseProgress);
                        context.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception("The CourseProgress does not exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Remove(int CourseProgressID)
        {
            try
            {
                CourseProgress CourseProgress = GetCourseProgressByID(CourseProgressID);
                if (CourseProgress != null)
                {
                    using (var context = new DBContext())
                    {
                        context.CourseProgresses.Remove(CourseProgress);
                        context.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception("The CourseProgress does not exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public CourseProgress GetCourseProgressByLearnerAndCourse(int learnerId, int courseId)
        {
            try
            {
                using var context = new DBContext();
                var courseProgress = context.CourseProgresses.FirstOrDefault(cp => cp.LearnerId == learnerId && cp.CourseId == courseId);
                return courseProgress;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}