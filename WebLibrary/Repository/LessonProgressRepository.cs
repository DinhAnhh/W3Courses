using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLibrary.DAO;
using WebLibrary.Models;

namespace WebLibrary.Repository
{
    public class LessonProgressRepository : ILessonProgressRepository
    {
        public LessonProgress GetLessonProgressByID(int LessonProgressId) => LessonProgressDAO.Instance.GetLessonProgressByID(LessonProgressId);
        public IEnumerable<LessonProgress> GetLessonProgresss() => LessonProgressDAO.Instance.GetLessonProgresslist();
        public void InsertLessonProgress(LessonProgress LessonProgress) => LessonProgressDAO.Instance.AddNew(LessonProgress);
        public void DeleteLessonProgress(int LessonProgressId) => LessonProgressDAO.Instance.Remove(LessonProgressId);
        public void UpdateLessonProgress(LessonProgress LessonProgress) => LessonProgressDAO.Instance.Update(LessonProgress);
        public LessonProgress GetLessonProgressByLessonAndChapter(int lessonId, int chapterId, int learnerId) => LessonProgressDAO.Instance.GetLessonProgressByLessonAndChapter(lessonId, chapterId, learnerId);
    }
}