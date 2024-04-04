using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLibrary.Models;

namespace WebLibrary.DAO
{
    public class ChapterProgressDAO
    {
        private static ChapterProgressDAO instance = null;
        private static readonly object instanceLock = new object();
        public static ChapterProgressDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ChapterProgressDAO();

                    }
                    return instance;
                }
            }
        }
        public IEnumerable<ChapterProgress> GetChapterProgresslist()
        {
            var ChapterProgress = new List<ChapterProgress>();
            try
            {
                using var context = new DBContext();
                ChapterProgress = context.ChapterProgresses.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

                throw;
            }
            return ChapterProgress;
        }

        public ChapterProgress GetChapterProgressByID(int ChapterProgressID)
        {
            ChapterProgress ChapterProgress = null;
            try
            { 
                using var context = new DBContext();
                ChapterProgress = context.ChapterProgresses.SingleOrDefault(c => c.ChapterProgressId.Equals(ChapterProgressID));

            }
            catch (System.Exception)
            {

                throw;
            }
            return ChapterProgress;
        }

        public void AddNew(ChapterProgress ChapterProgress)
        {
            try
            {
                ChapterProgress existingChapterProgress = GetChapterProgressByID(ChapterProgress.ChapterProgressId);
                if (existingChapterProgress == null)
                {
                    using (var context = new DBContext())
                    {
                        context.ChapterProgresses.Add(ChapterProgress);
                        context.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception("The ChapterProgress already exists.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Update(ChapterProgress ChapterProgress)
        {
            try
            {
                ChapterProgress existingChapterProgress = GetChapterProgressByID(ChapterProgress.ChapterProgressId);
                if (existingChapterProgress != null)
                {
                    using (var context = new DBContext())
                    {
                        context.ChapterProgresses.Update(ChapterProgress);
                        context.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception("The ChapterProgress does not exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Remove(int ChapterProgressID)
        {
            try
            {
                ChapterProgress ChapterProgress = GetChapterProgressByID(ChapterProgressID);
                if (ChapterProgress != null)
                {
                    using (var context = new DBContext())
                    {
                        context.ChapterProgresses.Remove(ChapterProgress);
                        context.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception("The ChapterProgress does not exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ChapterProgress GetChapterProgressByChapterAndCourseProgress(int chapterId, int courseProgressId)
        {
            try
            {
                using var context = new DBContext();
                var chapterProgress = context.ChapterProgresses.FirstOrDefault(cp => cp.ChapterId == chapterId && cp.CourseProgressId == courseProgressId);
                return chapterProgress;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}