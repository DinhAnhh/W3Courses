using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLibrary.Models;

namespace WebLibrary.DAO
{
   public class AnswerDAO
{
    private static AnswerDAO instance = null;
    private static readonly object instanceLock = new object();

    public static AnswerDAO Instance
    {
        get
        {
            lock (instanceLock)
            {
                if (instance == null)
                {
                    instance = new AnswerDAO();
                }
                return instance;
            }
        }
    }

    public IEnumerable<Answer> GetAnswerList()
    {
        var answers = new List<Answer>();
        try
        {
            using var context = new DBContext();
            answers = context.Answers.ToList();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return answers;
    }

    public Answer GetAnswerByID(int answerID)
    {
        Answer answer = null;
        try
        {
            using var context = new DBContext();
            answer = context.Answers.SingleOrDefault(a => a.AnswerId.Equals(answerID));
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return answer;
    }

    public void AddNew(Answer answer)
    {
        try
        {
            Answer existingAnswer = GetAnswerByID(answer.AnswerId);
            if (existingAnswer == null)
            {
                using (var context = new DBContext())
                {
                    context.Answers.Add(answer);
                    context.SaveChanges();
                }
            }
            else
            {
                throw new Exception("The answer already exists.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public void Update(Answer answer)
    {
        try
        {
            Answer existingAnswer = GetAnswerByID(answer.AnswerId);
            if (existingAnswer != null)
            {
                using (var context = new DBContext())
                {
                    context.Answers.Update(answer);
                    context.SaveChanges();
                }
            }
            else
            {
                throw new Exception("The answer does not exist.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public void Remove(int answerID)
    {
        try
        {
            Answer answer = GetAnswerByID(answerID);
            if (answer != null)
            {
                using (var context = new DBContext())
                {
                    context.Answers.Remove(answer);
                    context.SaveChanges();
                }
            }
            else
            {
                throw new Exception("The answer does not exist.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
}