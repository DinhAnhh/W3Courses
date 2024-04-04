using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLibrary.Models;

namespace Project_Group3.Models
{
    public class AnswerViewModels
    {
        public int QuizId { get; set; }
        public Course Course { get; set; }
           public bool UserCanSelectAnswer { get; set; } = true;
        public Chapter Chapter { get; set; }
        public Lesson Lesson { get; set; } 
        public List<Quiz> Quiz { get; set; }
        public string Note { get; set; }
        public IList<Answer> AnswerList { get; set; } = new List<Answer>();
    }
}