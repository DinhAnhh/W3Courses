using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Project_Group3.Models;
using WebLibrary.DAO;
using WebLibrary.Models;
using WebLibrary.Repository;

namespace Project_Group3.Controllers
{
    public class AdminController : Controller
    {

        AdminRepository adminRepository = null;
        IInstructorRepository instructorRepository = null;
        ICourseRepository courseRepository = null;
        ICategoryRepository categoryRepository = null;
        IInstructRepository instructRepository = null;
        ILearnerRepository learnerRepository = null;
        IChapterRepository chapterRepository = null;
        ILessonRepository lessonRepository = null;
        ISmtpRepository smtpRepository = null;
        IReportRepository reportRepository = null;
        IEnrollmentRepository enrollmentRepository = null;
public AdminController()
        {
            adminRepository = new AdminRepository();
            courseRepository = new CourseRepository();
            categoryRepository = new CategoryRepository();
            instructorRepository = new InstructorRepository();
            instructRepository = new InstructRepository();
            learnerRepository = new LearnerRepository();
            chapterRepository = new ChapterRepository();
            lessonRepository = new LessonRepository();
            smtpRepository = new StmpRepository();
            reportRepository = new ReportRepository();
            enrollmentRepository = new EnrollmentRepository();
        }

        public ActionResult Index()
        {
            var Adminlist = adminRepository.GetAdmins();
            var learnerList = learnerRepository.GetLearners();
            var instructorList = instructorRepository.GetInstructors();
            var courseList = courseRepository.GetCourses();
            var categoryList = categoryRepository.GetCategorys();
            var enrollmentList = enrollmentRepository.GetEnrollment();
            ModelsView model = new ModelsView{
                LearnersList = learnerList.ToList(),
                InstructorsList = instructorList.ToList(),
                CourseList = courseList.ToList(),
                CategoriesList = categoryList.ToList(),
                EnrollmentList = enrollmentList.ToList(),
            };
            return View(model);
        }

        public ActionResult Detail(int? id)
        {
            if (id == null) return NotFound();

            var Admin = adminRepository.GetAdminByID(id.Value);

            if (Admin == null) return NotFound();

            return View(Admin);
        }

        public ActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Admin Admin)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    adminRepository.InsertAdmin(Admin);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(Admin);
            }

        }

        public ActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            var Course = courseRepository.GetCourseByID(id.Value);

            if (Course == null) return NotFound();

            ModelsView modelsView = new ModelsView
            {
                Course = Course,
            };
            ViewBag.CategoryId = Course.CategoryId;
            var categoryList = categoryRepository.GetCategorys();
            ViewBag.CategoryList = new SelectList(categoryList, "CategoryId", "CategoryName");
            return View(modelsView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ModelsView modelsView)
        {
            try
            {
                var course = courseRepository.GetCourseByID(modelsView.Course.CourseId);
                if (course != null)
                {
                    if (ModelState.IsValid)
                    {
                        course.CreationDate = DateTime.Now;
                        courseRepository.UpdateCourse(modelsView.Course);
                    }
                    return RedirectToAction("Course", "Admin", new { id = Request.Cookies["ID"] });
                }
                return View(modelsView);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        public ActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            var Admin = adminRepository.GetAdminByID(id.Value);

            if (Admin == null) return NotFound();

            return View(Admin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                adminRepository.DeleteAdmin(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }

        }

        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("admin") != null) return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public IActionResult Login(string Username, string Password)
        {
            try
            {
                var admin = adminRepository.GetAdminByUsername(Username);
                System.Console.WriteLine(Password);
                if (admin != null && admin.Password == Password)
                {
                    HttpContext.Session.SetString("admin", Username);
                    ViewBag.Message = "Login successful";
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ViewBag.Message = "Login failed. Please check your credentials.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "An error occurred during login.";
                return View();
            }
        }

        public IActionResult Instructor(string search, bool showOnlyWait = false)
        {
            var instructorList = instructorRepository.GetInstructors();
            if (!string.IsNullOrEmpty(search))
            {
                string lowercaseSearch = search.ToLower();
                instructorList = instructorList.Where(i =>
                    i.FirstName.ToLower().Any(c => lowercaseSearch.Contains(c)) ||
                    i.LastName.ToLower().Any(c => lowercaseSearch.Contains(c))
                ).ToList();
                ViewBag.search = search;
            }
            ViewBag.Status = "";
            if (showOnlyWait)
            {
                ViewBag.Status = "Wait";
                instructorList = instructorList.Where(i => i.Status == "Wait").ToList();
            }
            return View(instructorList);
        }

        public IActionResult Learner(string search)
        {
            var learnerList = learnerRepository.GetLearners();
            if (!string.IsNullOrEmpty(search))
            {
                string lowercaseSearch = search.ToLower();
                learnerList = learnerList.Where(i =>
                    i.FirstName.ToLower().Any(c => lowercaseSearch.Contains(c)) ||
                    i.LastName.ToLower().Any(c => lowercaseSearch.Contains(c))
                ).ToList();
                ViewBag.search = search;
            }
            return View(learnerList);
        }

        public IActionResult Course(string search, bool showOnlyWait = false)
        {
            var course = courseRepository.GetCourses();
            var category = categoryRepository.GetCategorys();
            var instruct = instructRepository.GetInstructs();
            var instructor = instructorRepository.GetInstructors();
            var courseList = course.ToList();
            var categoryList = category.ToList();
            var instructList = instruct.ToList();
            var instructorList = instructor.ToList();
            ViewBag.Status = "";
            if (showOnlyWait)
            {
                ViewBag.Status = "Wait";
                courseList = courseList.Where(i => i.Status == "Wait").ToList();
            }
            return View(Tuple.Create(courseList, categoryList, instructList, instructorList));
        }

        public IActionResult UserDetail(int? id, string role)
        {
            if (id == null) return NotFound();

            var instructor = instructorRepository.GetInstructorByID(id.Value);
            var learner = learnerRepository.GetLearnerByID(id.Value);
            if (role.Equals("instructor"))
            {
                if (instructor == null) return NotFound();

                ViewBag.Role = role;
            }
            else if (role.Equals("learner"))
            {
                if (learner == null) return NotFound();

                ViewBag.Role = role;
            }
            return View(Tuple.Create(instructor, learner));
        }

        [HttpPost]
        public IActionResult Next(int id, string role)
        {
            if (role.Equals("instructor"))
            {
                var currentInstructor = instructorRepository.GetInstructorByID(id);
                var nextInstructor = instructorRepository.GetInstructors().FirstOrDefault(i => i.InstructorId > id);

                if (nextInstructor != null)
                {
                    return RedirectToAction("UserDetail", new { id = nextInstructor.InstructorId, role = role });
                }
                else
                {
                    var firstInstructor = instructorRepository.GetInstructors().FirstOrDefault();
                    return RedirectToAction("UserDetail", new { id = firstInstructor.InstructorId, role = role });
                }
            }
            else if (role.Equals("learner"))
            {
                var currentLearner = learnerRepository.GetLearnerByID(id);
                var nextLearner = learnerRepository.GetLearners().FirstOrDefault(l => l.LearnerId > id);

                if (nextLearner != null)
                {
                    return RedirectToAction("UserDetail", new { id = nextLearner.LearnerId, role = role });
                }
                else
                {
                    var firstLearner = learnerRepository.GetLearners().FirstOrDefault();
                    return RedirectToAction("UserDetail", new { id = firstLearner.LearnerId, role = role });
                }
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Previous(int id, string role)
        {
            if (role.Equals("instructor"))
            {
                var currentInstructor = instructorRepository.GetInstructorByID(id);
                var previousInstructor = instructorRepository.GetInstructors().LastOrDefault(i => i.InstructorId < id);

                if (previousInstructor != null)
                {
                    return RedirectToAction("UserDetail", new { id = previousInstructor.InstructorId, role = role });
                }
                else
                {
                    var lastInstructor = instructorRepository.GetInstructors().LastOrDefault();
                    return RedirectToAction("UserDetail", new { id = lastInstructor.InstructorId, role = role });
                }
            }
            else if (role.Equals("learner"))
            {
                var currentLearner = learnerRepository.GetLearnerByID(id);
                var previousLearner = learnerRepository.GetLearners().LastOrDefault(l => l.LearnerId < id);

                if (previousLearner != null)
                {
                    return RedirectToAction("UserDetail", new { id = previousLearner.LearnerId, role = role });
                }
                else
                {
                    var lastLearner = learnerRepository.GetLearners().LastOrDefault();
                    return RedirectToAction("UserDetail", new { id = lastLearner.LearnerId, role = role });
                }
            }
            return NotFound();
        }

        public IActionResult courseDetail(int? id)
        {
            if (id == null) return NotFound();

            var course = courseRepository.GetCourseByID(id.Value);
            var chapterList = chapterRepository.GetChapters();
            var lessonList = lessonRepository.GetLessons();
            var categoryList = categoryRepository.GetCategorys();
            var instruct = instructRepository.GetInstructs();
            var instructor = instructorRepository.GetInstructors();
            if (course == null) return NotFound();

            return View(Tuple.Create(course, chapterList, categoryList, instruct, lessonList, instructor));
        }

        public IActionResult AccountModeration(int? id)
        {
            if (id == null) return NotFound();

            var instructor = instructorRepository.GetInstructorByID(id.Value);

            if (instructor == null) return NotFound();

            return View(new ModelsView { Instructor = instructor });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AccountModeration(int id, ModelsView model)
        {
            try
            {
                if (id != model.Instructor.InstructorId) return NotFound();
                var instructor = instructorRepository.GetInstructorByID(model.Instructor.InstructorId);

                if (ModelState.IsValid)
                {
                    model.Instructor.Status = "Active";
                    if (instructorRepository.EditStatus(instructor.InstructorId, model.Instructor.Status))
                    {
                        smtpRepository.sendMail(instructor.Email, "Xác thực tài khoản", "Tài khoản của bạn đã được xác minh");
                        return RedirectToAction("Instructor", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("Instructor", "Admin");
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        public IActionResult CourseModeration(int? id)
        {
            if (id == null) return NotFound();

            var course = courseRepository.GetCourseByID(id.Value);
            var chapterList = chapterRepository.GetChapters();
            var lessonList = lessonRepository.GetLessons();
            var categoryList = categoryRepository.GetCategorys();
            var instructList = instructRepository.GetInstructs();
            var instructorList = instructorRepository.GetInstructors();
            System.Console.WriteLine(course.CourseId);
            if (course == null) return NotFound();
            ModelsView model = new ModelsView
            {
                Course = course,
                ChaptersList = chapterList.ToList(),
                LessonsList = lessonList.ToList(),
                CategoriesList = categoryList.ToList(),
                InstructsList = instructList.ToList(),
                InstructorsList = instructorList.ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CourseModeration(int id, ModelsView model)
        {
            System.Console.WriteLine(model.Course.CourseId);
            try
            {
                if (id != model.Course.CourseId) return NotFound();
                var course = courseRepository.GetCourseByID(model.Course.CourseId);

                if (ModelState.IsValid)
                {
                    model.Course.Status = "Active";
                    if (courseRepository.EditStatus(course.CourseId, model.Course.Status))
                    {
                        // smtpRepository.sendMail(course.in.Email, "Xác thực tài khoản", "Tài khoản của bạn đã được xác minh");
                        return RedirectToAction("Course", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("Course", "Admin");
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        public IActionResult Logout()
        {
            foreach (var cookie in HttpContext.Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
            HttpContext.Session.Clear();

            return RedirectToAction("Login", "Admin");
        }

        public IActionResult DeleteInstructor(int? id)
        {
            if (id == null) return NotFound();

            var instructor = instructorRepository.GetInstructorByID(id.Value);

            if (instructor == null) return NotFound();

            return View(instructor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteInstructor(int id)
        {
            try
            {
                instructorRepository.DeleteInstructor(id);
                TempData["DeleteSuccess"] = true;
                return RedirectToAction("Instructor", "Admin");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        public IActionResult DeleteLearner(int? id)
        {
            if (id == null) return NotFound();

            var learner = learnerRepository.GetLearnerByID(id.Value);

            if (learner == null) return NotFound();

            return View(learner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteLearner(int id)
        {
            try
            {
                learnerRepository.DeleteLearner(id);
                TempData["DeleteSuccess"] = true;
                return RedirectToAction("Learner", "Admin");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        [HttpGet]
        public ActionResult DeleteCourse(int? id)
        {
            if (id == null) return NotFound();

            var course = courseRepository.GetCourseByID(id.Value);

            if (course == null) return NotFound();

            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCourse(int id)
        {
            try
            {
                courseRepository.DeleteCourse(id);
                return RedirectToAction(nameof(Course));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }

        }
        public IActionResult Email()
        {
            var emailReportList = reportRepository.GetReportlist();
            return View(emailReportList);
        }

        public IActionResult Learning(int lessonId, int chapterId, int courseId, int instructorId)
        {

            var course = courseRepository.GetCourseByID(courseId);
            var chapter = chapterRepository.GetChapterByID(chapterId);
            var lesson = lessonRepository.GetLessonByID(lessonId);
            var chapterList = chapterRepository.GetChapters();
            var lessonList = lessonRepository.GetLessons();
            var instructor = instructorRepository.GetInstructorByID(instructorId);
            ViewBag.TimeLesson = lesson.Time;
            ViewBag.Course = course;
            ViewBag.CourseName = course.CourseName;
            ViewBag.CourseID = course.CourseId;
            ViewBag.ChapterID = chapter.ChapterId;
            ViewBag.InstructorID = instructor.InstructorId;

            return View(Tuple.Create(course, chapter, lesson, chapterList, lessonList, instructor));
        }
    }
}