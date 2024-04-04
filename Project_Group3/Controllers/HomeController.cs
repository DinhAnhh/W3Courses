using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Project_Group3.Models;
using WebLibrary.Models;
using WebLibrary.Repository;

namespace Project_Group3.Controllers
{
    public class HomeController : Controller
    {
        IInstructorRepository instructorRepository = null;
        ICourseRepository courseRepository = null;
        ICategoryRepository categoryRepository = null;
        IInstructRepository instructRepository = null;
        ILearnerRepository learnerRepository = null;
        IReviewRepository reviewRepository = null;
        ILessonRepository lessonRepository = null;
        IChapterRepository chapterRepository = null;
        IEnrollmentRepository enrollmentRepository = null;
        IReportRepository reportRepository = null;
        ISmtpRepository smtpRepository = null;
        ICourseProgressRepository courseProgressRepository = null;
        IChapterProgressRepository chapterProgressRepository = null;
        ILessonProgressRepository lessonProgressRepository = null;
        IQuizRepository quizRepository = null;

        public HomeController()
        {
            courseRepository = new CourseRepository();
            categoryRepository = new CategoryRepository();
            instructorRepository = new InstructorRepository();
            instructRepository = new InstructRepository();
            learnerRepository = new LearnerRepository();
            reviewRepository = new ReviewRepository();
            lessonRepository = new LessonRepository();
            chapterRepository = new ChapterRepository();
            enrollmentRepository = new EnrollmentRepository();
            reportRepository = new ReportRepository();
            smtpRepository = new StmpRepository();
            courseProgressRepository = new CourseProgressRepository();
            chapterProgressRepository = new ChapterProgressRepository();
            lessonProgressRepository = new LessonProgressRepository();
            quizRepository = new QuizRepository();
        }


        public IActionResult Index()
        {
            try
            {
                var categoryList = categoryRepository.GetCategorys().OrderByDescending(c => c.Courses.Count).Take(8).ToList();

                var instructorList = instructorRepository.GetInstructors().Take(4).ToList();

                return View(Tuple.Create(categoryList, instructorList));
            }
            catch (System.Exception)
            {
                return View();
            }

        }

        public IActionResult About() => View();

        public IActionResult Course(string search)
        {
            try
            {
                var course = courseRepository.GetCourses();
                var category = categoryRepository.GetCategorys();
                var instruct = instructRepository.GetInstructs();
                var instructor = instructorRepository.GetInstructors();
                var review = reviewRepository.GetReviews();
                var courseList = course.ToList();
                var categoryList = category.ToList();
                var instructList = instruct.ToList();
                var instructorList = instructor.ToList();
                var reviewList = review.ToList();
                var enrollment = enrollmentRepository.GetEnrollment();

                if (!string.IsNullOrEmpty(search))
                {
                    string lowercaseSearch = search.ToLower();
                    courseList = courseList.Where(c => c.CourseName.ToLower().Contains(lowercaseSearch)).ToList();
                    ViewBag.search = search;
                }
                return View(Tuple.Create(courseList, categoryList, instructList, instructorList, reviewList, enrollment));
            }
            catch (System.Exception)
            {
                return View();
            }
        }

        public IActionResult Contact() => View();

        [HttpPost]
        public IActionResult Contact(Report report)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    report.SubmittedTime = DateTime.Now;
                    reportRepository.AddNew(report);
                    smtpRepository.sendMail("huynhnguyenbao3105@gmail.com", report.Title, "My name is " + report.Name + ". My Email is: " + report.Email + " I wanna say " + report.Content);

                    return View();
                }
                return View();
            }
            catch (System.Exception)
            {
                return View();
            }
        }

       public IActionResult InstructorProfile(int? id)
        {
            if (id == null) return NotFound();

            Instructor instructor = instructorRepository.GetInstructorByID(id.Value);

            if (instructor == null) return NotFound();

            ViewBag.Role = "instructor";

            ModelsView modelsView = new ModelsView
            {
                Instructor = instructor,
            };

            return View(modelsView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InstructorProfile(int id, ModelsView models, IFormFile picture)
        {
            try
            {
                if (id != models.Instructor.InstructorId)
                {
                    System.Console.WriteLine(id + " " + models.Instructor.InstructorId);
                    return NotFound();
                }
                if (ModelState.IsValid) 
                {
                    if (picture != null && picture.Length > 0)
                    {
                        var urlRelative = "/img/avatars/";
                        var urlAbsolute = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "avatars");
                        var fileName = Guid.NewGuid() + Path.GetExtension(picture.FileName);
                        var filePath = Path.Combine(urlAbsolute, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            picture.CopyTo(stream);
                        }
                        models.Instructor.Picture = Path.Combine(urlRelative, fileName);
                    }
                    instructorRepository.UpdateInstructor(models.Instructor);
                }
                return RedirectToAction("InstructorProfile", "Home", new { id = models.Instructor.InstructorId });
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        public IActionResult LearnerProfile(int? id)
        {
            var enrollment = enrollmentRepository.GetEnrollment();

            if (id == null) return NotFound();

            Learner learner = learnerRepository.GetLearnerByID(id.Value);

            if (learner == null) return NotFound();

            ViewBag.Role = "learner";

            ModelsView modelsView = new ModelsView
            {
                Learner = learner,
                EnrollmentList = enrollment.ToList(),
            };

            return View(modelsView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LearnerProfile(int id, ModelsView models, IFormFile picture)
        {
            try
            {
                if (id != models.Learner.LearnerId)
                {
                    System.Console.WriteLine(id + " " + models.Learner.LearnerId);
                    return NotFound();
                }if (ModelState.IsValid)
                {
                    if (picture != null && picture.Length > 0)
                    {
                        var urlRelative = "/img/avatars/";
                        var urlAbsolute = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "avatars");
                        var fileName = Guid.NewGuid() + Path.GetExtension(picture.FileName);
                        var filePath = Path.Combine(urlAbsolute, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            picture.CopyTo(stream);
                        }
                        models.Learner.Picture = Path.Combine(urlRelative, fileName);
                    }
                    learnerRepository.UpdateLearner(models.Learner);
                }

                return RedirectToAction("LearnerProfile", "Home", new { id = models.Learner.LearnerId });
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAccountLearner(Learner learner)
        {
            try
            {
                var i = learnerRepository.GetLearnerByID(learner.LearnerId);
                if (ModelState.IsValid)
                {
                    learner.FirstName = i.FirstName;
                    learner.LastName = i.LastName;
                    learner.Gender = i.Gender;
                    learner.Birthday = i.Birthday;
                    learner.PhoneNumber = i.PhoneNumber;
                    learner.Email = i.Email;
                    learner.Country = i.Country;
                    learner.Username = i.Username;
                    learner.Password = i.Password;
                    learner.Picture = i.Picture;
                    learner.RegistrationDate = i.RegistrationDate;
                    learner.Status = "Delete";
                    learnerRepository.UpdateLearner(learner);
                }
                foreach (var cookie in HttpContext.Request.Cookies.Keys)
                {
                    Response.Cookies.Delete(cookie);
                }

                HttpContext.Session.Clear();

                return RedirectToAction("Login", "User");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAccountInstructor(Instructor instructor)
        {
            try
            {
                var i = instructorRepository.GetInstructorByID(instructor.InstructorId);
                if (ModelState.IsValid)
                {
                    instructor.FirstName = i.FirstName;
                    instructor.LastName = i.LastName;instructor.Gender = i.Gender;
                    instructor.Birthday = i.Birthday;
                    instructor.PhoneNumber = i.PhoneNumber;
                    instructor.Email = i.Email;
                    instructor.Country = i.Country;
                    instructor.Username = i.Username;
                    instructor.Password = i.Password;
                    instructor.Picture = i.Picture;
                    instructor.Income = i.Income;
                    instructor.Introduce = i.Introduce;
                    instructor.RegistrationDate = i.RegistrationDate;
                    instructor.Status = "Delete";
                    instructorRepository.UpdateInstructor(instructor);
                }
                foreach (var cookie in HttpContext.Request.Cookies.Keys)
                {
                    Response.Cookies.Delete(cookie);
                }

                HttpContext.Session.Clear();

                return RedirectToAction("Login", "User");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                var learner = learnerRepository.GetLearnerByID(id);

                if (learner == null) return NotFound();

                learner.Status = "Delete";

                learnerRepository.UpdateLearner(learner);

                return RedirectToAction("Instructor", "Admin");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        public IActionResult CourseDetail(int? id)
        {
            try
            {
                if (id == null) return NotFound();

                var instruct = instructRepository.GetInstructByID(id.Value);

                if (instruct == null) return NotFound();

                var courseId = TempData["CourseId"] as string;
                var course = courseRepository.GetCourses();
                var instructor = instructorRepository.GetInstructors();
                var review = reviewRepository.GetReviews();
                var learner = learnerRepository.GetLearners();
                var chapter = chapterRepository.GetChapters();
                var lesson = lessonRepository.GetLessons();
                var enrollment = enrollmentRepository.GetEnrollment();

                var courseInfo = course.FirstOrDefault(c => c.CourseId == instruct.CourseId);
                var instructorInfo = instructor.FirstOrDefault(i => i.InstructorId == instruct.InstructorId);
                ViewBag.CourseID = id;
                ModelsView modelsView = new ModelsView
                {
                    Course = courseInfo,
                    Instructor = instructorInfo,
                    ReviewsList = review.ToList(),
                    LearnersList = learner.ToList(),
                    ChaptersList = chapter.ToList(),
                    LessonsList = lesson.ToList(),
                    EnrollmentList = enrollment.ToList(),
                };

                return View(modelsView);
            }
            catch (System.Exception)
            {
                return View();
            }
        }

      public IActionResult Learning(int lessonId, int chapterId, int courseId)
        {
            try
            {
                var cookieValue = Request.Cookies["ID"];
                var learnerId = Convert.ToInt32(cookieValue);
                var courseProgress = courseProgressRepository.GetCourseProgressByLearnerAndCourse(learnerId, courseId);
                if (courseProgress == null)
                {
                    courseProgress = new CourseProgress
                    {
                        LearnerId = learnerId,
                        CourseId = courseId,
                        Completed = false,
                        ProgressPercent = 0,
                        Rated = false,
                        TotalTime = 0,
                        StartAt = DateTime.Now
                    };
                    courseProgressRepository.InsertCourseProgress(courseProgress);
                }

                var chapterProgress = chapterProgressRepository.GetChapterProgressByChapterAndCourseProgress(chapterId, courseProgress.CourseProgressId);
                if (chapterProgress == null)
                {
                   

                    chapterProgress = new ChapterProgress
                    {
                        ChapterId = chapterId,
                        CourseProgressId = courseProgress.CourseProgressId,
                        Completed = false,
                        ProgressPercent = 0,
                        TotalTime = 0,
                        StartAt = DateTime.Now
                    };
                     var chapter1 = chapterRepository.GetChapterByID(chapterProgress.ChapterId.Value);
                     

                    string learnerID = HttpContext.Session.GetString("username");
                    System.Console.WriteLine($"learnerId = {learnerID}");
                    var learner1 = learnerRepository.GetLearnerByUser(learnerID);
                    smtpRepository.sendMail(learner1.Email, $"You have successfully complete chapter{chapter1.ChapterName} ", "Thank you for your kind words! I'm glad to hear that our efforts are contributing to the improvement of teaching quality. Continuous efforts and dedication are key to achieving positive outcomes in any endeavor, including education. If you have any specific questions or need further assistance, feel free to ask.");
                    System.Console.WriteLine(chapter1.ChapterName);

                    chapterProgressRepository.InsertChapterProgress(chapterProgress);
                }

                var lessonProgress = lessonProgressRepository.GetLessonProgressByLessonAndChapter(lessonId, chapterId, learnerId);
                if (lessonProgress == null)
                {
                    lessonProgress = new LessonProgress
                    {
                        LearnerId = learnerId,
                        LessonId = lessonId,
                        ChapterId = chapterId,
                        Completed = false,
StartAt = DateTime.Now
                    };
                    lessonProgressRepository.InsertLessonProgress(lessonProgress);
                }

                var course = courseRepository.GetCourseByID(courseId);
                var chapter = chapterRepository.GetChapterByID(chapterId);
                var lesson = lessonRepository.GetLessonByID(lessonId);
                var chapterList = chapterRepository.GetChapters();
                var lessonList = lessonRepository.GetLessons();
                var learner = learnerRepository.GetLearners();
                ViewBag.TimeLesson = lesson.Time;
                ViewBag.Course = course;
                ViewBag.CourseName = course.CourseName;
                ViewBag.CourseID = course.CourseId;
                ViewBag.ChapterID = chapter.ChapterId;
                ViewBag.LessonID = lesson.LessonId;

                return View(Tuple.Create(chapter, lesson, courseProgress, chapterProgress, lessonProgress, chapterList, lessonList));
            }
            catch (System.Exception)
            {
                return View();
            }
        }
        public IActionResult CheckOut(int? id)
        {
            try
            {
                var learner = learnerRepository.GetLearnerByID(id.Value);

                return View(learner);
            }
            catch (System.Exception)
            {
                return View();
            }
        }

        public IActionResult QA() => View();

        [HttpPost]
        public ActionResult UpdateCheckValue(int lessonId, int chapterId, int courseId)
        {
            try
            {
                int learnerId = int.Parse(Request.Cookies["ID"]);
                var course = courseRepository.GetCourseByID(courseId);
                var chapter = chapterRepository.GetChapterByID(chapterId);
                var lesson = lessonRepository.GetLessonByID(lessonId);
                var lessonProgress = lessonProgressRepository.GetLessonProgressByLessonAndChapter(lessonId, chapterId, courseId);
                var courseProgress = courseProgressRepository.GetCourseProgressByLearnerAndCourse(learnerId, courseId);
                var chapterProgress = chapterProgressRepository.GetChapterProgressByChapterAndCourseProgress(chapterId, courseProgress.CourseProgressId);

                lessonProgress.Completed = true;
                lessonProgressRepository.InsertLessonProgress(lessonProgress);

                return Content("success");
            }
            catch (System.Exception)
            {
                return View();
            }

        }
    }
}
