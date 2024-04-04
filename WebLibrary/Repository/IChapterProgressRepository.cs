using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLibrary.Models;

namespace WebLibrary.Repository
{
    public interface IChapterProgressRepository
    {
          IEnumerable<ChapterProgress> GetChapterProgresss();
        ChapterProgress GetChapterProgressByID(int ChapterProgressId);
        void InsertChapterProgress(ChapterProgress ChapterProgress);
        void DeleteChapterProgress(int ChapterProgressId);
        void UpdateChapterProgress(ChapterProgress ChapterProgress);
        ChapterProgress GetChapterProgressByChapterAndCourseProgress(int chapterId, int courseProgressId);
    }
}