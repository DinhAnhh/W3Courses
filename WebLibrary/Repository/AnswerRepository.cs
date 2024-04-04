using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLibrary.DAO;
using WebLibrary.Models;

namespace WebLibrary.Repository
{
    public class AnswerRepository : IAnswerRepository
    {
        public Answer GetAnswerByID(int answerId) => AnswerDAO.Instance.GetAnswerByID(answerId);

        public IEnumerable<Answer> GetAnswers() => AnswerDAO.Instance.GetAnswerList();

        public void InsertAnswer(Answer answer) => AnswerDAO.Instance.AddNew(answer);

        public void DeleteAnswer(int answerId) => AnswerDAO.Instance.Remove(answerId);

        public void UpdateAnswer(Answer answer) => AnswerDAO.Instance.Update(answer);

    }
}