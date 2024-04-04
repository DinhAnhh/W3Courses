using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLibrary.DAO;
using WebLibrary.Models;

namespace WebLibrary.Repository
{
  public class QuizRepository : IQuizRepository
{
    public Quiz GetQuizByID(int quizId) => QuizDAO.Instance.GetQuizByID(quizId);

    public IEnumerable<Quiz> GetQuizzes() => QuizDAO.Instance.GetQuizList();

    public void InsertQuiz(Quiz quiz) => QuizDAO.Instance.AddNew(quiz);

    public void DeleteQuiz(int quizId) => QuizDAO.Instance.Remove(quizId);

    public void UpdateQuiz(Quiz quiz) => QuizDAO.Instance.Update(quiz);

       
    }
}