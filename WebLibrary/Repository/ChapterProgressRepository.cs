using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLibrary.DAO;
using WebLibrary.Models;

namespace WebLibrary.Repository
{
    public class ChapterProgressRepository : IChapterProgressRepository
    {
        public ChapterProgress GetChapterProgressByID(int ChapterProgressId) => ChapterProgressDAO.Instance.GetChapterProgressByID(ChapterProgressId);
        public IEnumerable<ChapterProgress> GetChapterProgresss() => ChapterProgressDAO.Instance.GetChapterProgresslist();
        public void InsertChapterProgress(ChapterProgress ChapterProgress) => ChapterProgressDAO.Instance.AddNew(ChapterProgress);
        public void DeleteChapterProgress(int ChapterProgressId) => ChapterProgressDAO.Instance.Remove(ChapterProgressId);
        public void UpdateChapterProgress(ChapterProgress ChapterProgress) => ChapterProgressDAO.Instance.Update(ChapterProgress);
        public ChapterProgress GetChapterProgressByChapterAndCourseProgress(int chapterId, int courseProgressId) => ChapterProgressDAO.Instance.GetChapterProgressByChapterAndCourseProgress(chapterId, courseProgressId);
    }
}