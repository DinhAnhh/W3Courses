using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project_Group3.Models;
using WebLibrary.Models;
using WebLibrary.Repository;
namespace Project_Group3.Controllers
{


    public class QuizController : Controller
    {
        IQuizRepository quizRepository = null;
        IAnswerRepository answerRepository = null;
        IChapterRepository chapterRepository = null;
        ICourseRepository courseRepository = null;
        ILessonRepository lessonRepository = null;
        ILessonProgressRepository lessonProgressRepository = null;
        ILearnerRepository learnerRepository = null;
        ISmtpRepository smtpRepository = null;
        public QuizController()
        {
            quizRepository = new QuizRepository();
            answerRepository = new AnswerRepository();
            chapterRepository = new ChapterRepository();
            courseRepository = new CourseRepository();
            lessonRepository = new LessonRepository();
            lessonProgressRepository = new LessonProgressRepository();
            learnerRepository = new LearnerRepository();
            smtpRepository = new StmpRepository();
        }


        public ActionResult Index(int chapterId, int courseId)
        {
            try
            {
                var chapter = chapterRepository.GetChapterByID(chapterId);
                var course = courseRepository.GetCourseByID(courseId);
                var quizList = quizRepository.GetQuizzes();
                var answerList = answerRepository.GetAnswers();
                var quizzesToDisplay = quizList.Where(q => q.ChapterId == chapterId && q.CourseId == courseId).ToList();
                var model = Tuple.Create((IEnumerable<WebLibrary.Models.Quiz>)quizzesToDisplay, answerList, chapter, course);
                return View(model);
            }
            catch (System.Exception)
            {
                return View();
            }
        }

        public ActionResult Create(int chapterId, int courseId)
        {
            try
            {
                var chapter = chapterRepository.GetChapterByID(chapterId);
                var course = courseRepository.GetCourseByID(courseId);

                if (chapter == null || course == null) return RedirectToAction("Error");

                ViewBag.ChapterId = chapterId;
                ViewBag.CourseId = courseId;
                ViewBag.ChapterName = chapter.ChapterName;
                ViewBag.CourseName = course.CourseName;
                ViewBag.Answers = answerRepository.GetAnswers();
                return View();
            }
            catch (System.Exception)
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Quiz quiz, int chapterId, int courseId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(quiz.Quiz1))
                    {
                        ModelState.AddModelError("Quiz1", "Quiz1 is required.");
                    }

                    if (string.IsNullOrEmpty(quiz.DapAnA))
                    {
                        ModelState.AddModelError("DapAnA", "DapAnA is required.");
                    }

                    if (string.IsNullOrEmpty(quiz.DapAnB))
                    {
                        ModelState.AddModelError("DapAnB", "DapAnB is required.");
                    }

                    if (string.IsNullOrEmpty(quiz.DapAnC))
                    {
                        ModelState.AddModelError("DapAnC", "DapAnC is required.");
                    }

                    if (string.IsNullOrEmpty(quiz.DapAnD))
                    {
                        ModelState.AddModelError("DapAnD", "DapAnD is required.");
                    }

                    if (quiz.AnswerId == 0)
                    {
                        ModelState.AddModelError("AnswerId", "Please select an answer.");
                    }

                    if (ModelState.IsValid)
                    {
                        quizRepository.InsertQuiz(quiz);
                        return RedirectToAction("Index", new { chapterId = chapterId, courseId = courseId });
                    }
                }

                var answerList = answerRepository.GetAnswers();
                ViewBag.Answers = new SelectList(answerList, "AnswerId", "Answer1");
                return View(quiz);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                var chapter = chapterRepository.GetChapterByID(chapterId);
                var course = courseRepository.GetCourseByID(courseId);
                ViewBag.CourseId = courseId;
                ViewBag.ChapterId = chapterId;
                ViewBag.ChapterName = chapter.ChapterName;
                ViewBag.CourseName = course.CourseName;
                var answerList = answerRepository.GetAnswers();
                ViewBag.Answers = new SelectList(answerList, "AnswerId", "Answer1");
                return View(quiz);
            }
        }


public IActionResult Test(int chapterId, int courseId, int lessonId, int numberOfQuestions, int correctCount)
        {
            var allQuizzes = quizRepository.GetQuizzes();
            var chapter = chapterRepository.GetChapterByID(chapterId);
            var course = courseRepository.GetCourseByID(courseId);
            ViewBag.LessonId = lessonId;
            var answerList = answerRepository.GetAnswers();
            var quizList = allQuizzes.Where(q => q.ChapterId == chapterId && q.CourseId == courseId);

            if (quizList.Count() < numberOfQuestions)
            {
                return RedirectToAction("Error");
            }

            var selectedAnswers = new List<Quiz>();

            foreach (var quiz in quizList)
            {
                var savedQuiz = allQuizzes.FirstOrDefault(q =>
                    q.Quiz1 == quiz.Quiz1 &&
                    q.DapAnA == quiz.DapAnA &&
                    q.DapAnB == quiz.DapAnB &&
                    q.DapAnC == quiz.DapAnC &&
                    q.DapAnD == quiz.DapAnD);

                if (savedQuiz != null)
                {
                    quiz.AnswerId = savedQuiz.AnswerId;
                }

                selectedAnswers.Add(quiz);
            }

            var result = new AnswerViewModels()
            {
                Chapter = chapter,
                Course = course,
                Quiz = selectedAnswers,
                AnswerList = answerList.ToList(),
            };

            ViewBag.CorrectCount = correctCount;
            ViewBag.TotalCount = numberOfQuestions;

            return View(result);
        }

        [HttpPost]
        public IActionResult Test(AnswerViewModels model, int lessonId)
        {
            var chapter = chapterRepository.GetChapterByID(model.Chapter.ChapterId);
            var course = courseRepository.GetCourseByID(model.Course.CourseId);
            var quizList = model.Quiz;
            int correctCount = 0;
            int wrongCount = 0;
            ViewBag.LessonId = lessonId;

            foreach (var quiz in quizList)
            {
                var selectedAnswerId = quiz.AnswerId;
                var correctAnswerId = quizRepository.GetQuizByID(quiz.QuizId)?.AnswerId;
                quiz.IsCorrect = selectedAnswerId == correctAnswerId;

                if (quiz.IsCorrect.HasValue && quiz.IsCorrect.Value)
                {
                    correctCount++;
                }
                else
                {
                    wrongCount++;
                }
            }

            model.UserCanSelectAnswer = false;
            var result = new QuizResultViewModel
            {
                CorrectCount = correctCount,
                WrongCount = wrongCount,
                Chapter = chapter,
                Course = course,
                QuizList = quizList,
            };
            return ProcessPostResult(result);
        }
public IActionResult ProcessPostResult(QuizResultViewModel model)
{
    var chapters = chapterRepository.GetChapters()
        .Where(c => c.CourseId == model.Course.CourseId)
        .OrderBy(c => c.Index)
        .ToList();
    var lessonId = ViewBag.LessonId;
    var currentChapterIndex = chapters.FindIndex(c => c.ChapterId == model.Chapter.ChapterId);

    if (model.CorrectCount > model.WrongCount)
    {
        if (currentChapterIndex == chapters.Count - 1)
        {
            // Scenario 1: No more lessons in the current chapter
            return RedirectToAction("CourseDetail", "Home", new { Id = model.Course.CourseId });
        }
        else if (currentChapterIndex != -1 && currentChapterIndex < chapters.Count - 1)
        {
            var nextChapter = chapters[currentChapterIndex + 1];
            var firstLessonOfNextChapter = lessonRepository.GetLessons()
                .FirstOrDefault(l => l.ChapterId == nextChapter.ChapterId);
            string learnerId = HttpContext.Session.GetString("username");
            System.Console.WriteLine($"learnerId = {learnerId}");
            var learner = learnerRepository.GetLearnerByUser(learnerId);
            smtpRepository.sendMail(learner.Email, $"You have successfully complete chapter{model.Chapter.ChapterName} ", "Thank you for your kind words! I'm glad to hear that our efforts are contributing to the improvement of teaching quality. Continuous efforts and dedication are key to achieving positive outcomes in any endeavor, including education. If you have any specific questions or need further assistance, feel free to ask.");
            System.Console.WriteLine(model.Chapter.ChapterName);
            if (firstLessonOfNextChapter != null)
            {
                // Scenario 2: Next chapter has lessons
                return RedirectToAction("Learning", "Home", new { courseId = model.Course.CourseId, chapterId = nextChapter.ChapterId, lessonId = firstLessonOfNextChapter.LessonId });
            }
            else
            {
                // Scenario 3: Next chapter has only a test (no lessons)
                var nextChapterTest = quizRepository.GetQuizzes().FirstOrDefault(q => q.ChapterId == nextChapter.ChapterId);
                if (nextChapterTest != null)
                {
                    return RedirectToAction("Test", "Quiz", new
                    {
                        chapterId = nextChapter.ChapterId,
                        courseId = model.Course.CourseId,
                        lessonId = lessonId,
                        numberOfQuestions = nextChapterTest.QuizId, // Số lượng câu hỏi của bài test
                        correctCount = model.CorrectCount,
                        totalCount = model.CorrectCount + model.WrongCount,
                    });
                }
                else
                {
                    // Scenario 4: Next chapter has neither lessons nor test
return RedirectToAction("CourseDetail", "Home", new { Id = model.Course.CourseId });
                }
            }
        }
    }

    // Default scenario: Incorrect answers
    ViewBag.WrongAnswerMessage = "Bạn có một số câu trả lời sai. Vui lòng làm lại bài test.";
    return RedirectToAction("Test", "Quiz", new
    {
        chapterId = model.Chapter.ChapterId,
        courseId = model.Course.CourseId,
        lessonId = lessonId,
        numberOfQuestions = model.QuizList.Count,
        correctCount = model.CorrectCount,
        totalCount = model.CorrectCount + model.WrongCount,
    });
}

        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null) return NotFound();

                var quiz = quizRepository.GetQuizByID(id.Value);

                if (quiz == null) return NotFound();

                ModelsView modelsView = new ModelsView { Quiz = quiz };

                ViewBag.AnswerId = quiz.AnswerId; // Add this line to pass the answers to the view
                var answerList = answerRepository.GetAnswers();
                ViewBag.AnswerList = new SelectList(answerList, "AnswerId", "Answer1");
                return View(modelsView);
            }
            catch (System.Exception)
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ModelsView modelsView)
        {
            try
            {
                var quiz = quizRepository.GetQuizByID(modelsView.Quiz.QuizId);
                if (quiz != null)
                {
                    quizRepository.UpdateQuiz(modelsView.Quiz);
                    return RedirectToAction("Index", new { chapterId = quiz.ChapterId, courseId = quiz.CourseId });
                }

                ViewBag.Answers = answerRepository.GetAnswers();
                return View(modelsView);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(modelsView);
            }
        }
        public ActionResult Error() => View();

        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null) return NotFound();

                var Quiz = quizRepository.GetQuizByID(id.Value);

                var Answer = answerRepository.GetAnswerByID(Quiz.AnswerId.Value);

                if (Quiz == null) return NotFound();

                return View(new ModelsView { Quiz = Quiz, Answer = Answer });
            }
            catch (System.Exception)
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ModelsView modelsView)
        {
            try
            {
                var quiz = quizRepository.GetQuizByID(modelsView.Quiz.QuizId);
                if (quiz != null)
                {
                    quizRepository.DeleteQuiz(quiz.QuizId);
                    return RedirectToAction("Index", new { chapterId = quiz.ChapterId, courseId = quiz.CourseId });
                }
                return View(modelsView);
            }
            catch (Exception ex)

            {
                ViewBag.Message = ex.Message;
                return View(modelsView);
            }

        }
    }
}
