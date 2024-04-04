using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLibrary.Models;
namespace WebLibrary.DAO
{
    public class QuizDAO
{
    private static QuizDAO instance = null;
    private static readonly object instanceLock = new object();

    public static QuizDAO Instance
    {
        get
        {
            lock (instanceLock)
            {
                if (instance == null)
                {
                    instance = new QuizDAO();
                }
                return instance;
            }
        }
    }

    public IEnumerable<Quiz> GetQuizList()
    {
        var quizzes = new List<Quiz>();
        try
        {
            using var context = new DBContext();
            quizzes = context.Quizzes.ToList();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return quizzes;
    }

    public Quiz GetQuizByID(int quizID)
    {
        Quiz quiz = null;
        try
        {
            using var context = new DBContext();
            quiz = context.Quizzes.SingleOrDefault(q => q.QuizId.Equals(quizID));
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return quiz;
    }

    public void AddNew(Quiz quiz)
    {
        try
        {
            Quiz existingQuiz = GetQuizByID(quiz.QuizId);
            if (existingQuiz == null)
            {
                using (var context = new DBContext())
                {
                    context.Quizzes.Add(quiz);
                    context.SaveChanges();
                }
            }
            else
            {
                throw new Exception("The quiz already exists.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public void Update(Quiz quiz)
    {
        try
        {
            Quiz existingQuiz = GetQuizByID(quiz.QuizId);
            if (existingQuiz != null)
            {
                using (var context = new DBContext())
                {
                    context.Quizzes.Update(quiz);
                    context.SaveChanges();
                }
            }
            else
            {
                throw new Exception("The quiz does not exist.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public void Remove(int quizID)
    {
        try
        {
            Quiz quiz = GetQuizByID(quizID);
            if (quiz != null)
            {
                using (var context = new DBContext())
                {
                    context.Quizzes.Remove(quiz);
                    context.SaveChanges();
                }
            }
            else
            {
                throw new Exception("The quiz does not exist.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
}