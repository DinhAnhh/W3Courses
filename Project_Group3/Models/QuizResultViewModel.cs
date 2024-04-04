using System.Collections.Generic;
using WebLibrary.Models;

namespace Project_Group3.Models
{
    public class QuizResultViewModel
    {
        public int CorrectCount { get; set; }
        public int WrongCount { get; set; }
        public Chapter Chapter { get; set; }
        public Course Course { get; set; }
        public Lesson Lesson { get; set; }
        public List<Quiz> QuizList { get; set; }

    }

}