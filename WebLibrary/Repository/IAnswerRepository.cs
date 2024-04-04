using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLibrary.Models;

namespace WebLibrary.Repository
{
   public interface IAnswerRepository
{
    IEnumerable<Answer> GetAnswers();
    Answer GetAnswerByID(int answerId);
    void InsertAnswer(Answer answer);
    void DeleteAnswer(int answerId);
    void UpdateAnswer(Answer answer);
}
}