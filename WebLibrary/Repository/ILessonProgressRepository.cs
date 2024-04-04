using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLibrary.Models;

namespace WebLibrary.Repository
{
    public interface ILessonProgressRepository
    {
          IEnumerable<LessonProgress> GetLessonProgresss();
        LessonProgress GetLessonProgressByID(int LessonProgressId);
        void InsertLessonProgress(LessonProgress LessonProgress);
        void DeleteLessonProgress(int LessonProgressId);
        void UpdateLessonProgress(LessonProgress LessonProgress);
        LessonProgress GetLessonProgressByLessonAndChapter(int lessonId, int chapterId, int learnerId);
    }
}