using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebLibrary.Models;

namespace WebLibrary.DAO
{
    public class LessonProgressDAO
    {
        private static LessonProgressDAO instance = null;
        private static readonly object instanceLock = new object();
        public static LessonProgressDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new LessonProgressDAO();

                    }
                    return instance;
                }
            }
        }
        public IEnumerable<LessonProgress> GetLessonProgresslist()
        {
            var lessonProgress = new List<LessonProgress>();
            try
            {
                using var context = new DBContext();
                lessonProgress = context.LessonProgresses.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

                throw;
            }
            return lessonProgress;
        }

        public LessonProgress GetLessonProgressByID(int lessonProgressID)
        {
            LessonProgress lessonProgress = null;
            try
            {
                using var context = new DBContext();
                lessonProgress = context.LessonProgresses.SingleOrDefault(c => c.LessonProgressId.Equals(lessonProgressID));

            }
            catch (System.Exception)
            {

                throw;
            }
            return lessonProgress;
        }

        public void AddNew(LessonProgress lessonProgress)
        {
            try
            {
                LessonProgress existingLessonProgress = GetLessonProgressByID(lessonProgress.LessonProgressId);
                if (existingLessonProgress == null)
                {
                    using (var context = new DBContext())
                    {
                        context.LessonProgresses.Add(lessonProgress);
                        context.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception("The lessonProgress already exists.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Update(LessonProgress lessonProgress)
        {
            try
            {
                LessonProgress existingLessonProgress = GetLessonProgressByID(lessonProgress.LessonProgressId);
                if (existingLessonProgress != null)
                {
                    using (var context = new DBContext())
                    {
                        context.LessonProgresses.Update(lessonProgress);
                        context.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception("The lessonProgress does not exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Remove(int lessonProgressID)
        {
            try
            {
                LessonProgress lessonProgress = GetLessonProgressByID(lessonProgressID);
                if (lessonProgress != null)
                {
                    using (var context = new DBContext())
                    {
                        context.LessonProgresses.Remove(lessonProgress);
                        context.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception("The lessonProgress does not exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public LessonProgress GetLessonProgressByLessonAndChapter(int lessonId, int chapterId, int learnerId)
        {
            try
            {
                using var context = new DBContext();
                var lessonProgress = context.LessonProgresses.FirstOrDefault(lp => lp.LessonId == lessonId && lp.ChapterId == chapterId && lp.LearnerId == learnerId);
                return lessonProgress;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}