using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLibrary.Models;

namespace WebLibrary.Repository
{
   public interface IQuizRepository
{
    IEnumerable<Quiz> GetQuizzes();
    Quiz GetQuizByID(int quizId);
    void InsertQuiz(Quiz quiz); 
    void DeleteQuiz(int quizId);
    void UpdateQuiz(Quiz quiz);
}
}