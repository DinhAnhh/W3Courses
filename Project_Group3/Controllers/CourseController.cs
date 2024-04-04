using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Project_Group3.Models;
using WebLibrary.Models;
using WebLibrary.Repository;
namespace Project_Group3.Controllers
{

    public class CourseController : Controller
    {
        ICourseRepository courseRepository = null;
        ICategoryRepository categoryRepository = null;
        IChapterRepository chapterRepository = null;
        ILessonRepository lessonRepository = null;
        IInstructorRepository instructorRepository = null;
        IInstructRepository instructRepository = null;
        private int idInstructor;

        public CourseController()
        {
            courseRepository = new CourseRepository();
            categoryRepository = new CategoryRepository();
            chapterRepository = new ChapterRepository();
            lessonRepository = new LessonRepository();
            instructorRepository = new InstructorRepository();
            instructRepository = new InstructRepository();
        }

        public IActionResult Index(int id, string search = "", int page = 1)
        {
            try
            {
                idInstructor = int.Parse(HttpContext.Session.GetInt32("InsID").GetValueOrDefault().ToString());
                var instructor = instructorRepository.GetInstructorByID(id);
                if (instructor == null) return NotFound();

                var courseList = courseRepository.GetCourses();
                var chapterList = chapterRepository.GetChapters();
                var categoryList = categoryRepository.GetCategorys();
                var instructList = instructRepository.GetInstructs();

                if (!string.IsNullOrEmpty(search))
                {
                    courseList = courseList.Where(c => c.CourseName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                }

                ModelsView modelsView = new ModelsView
                {
                    CourseList = courseList.ToList(),
                    ChaptersList = chapterList.ToList(),
                    CategoriesList = categoryList.ToList(),
                    Instructor = instructor,
                    InstructsList = instructList.ToList(),
                };

                ViewBag.Search = search;
                ViewBag.CurrentPage = page;
                ViewBag.InstructorID = id;

                return View(modelsView);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while processing your request.";
                return View();
            }
        }

        public ActionResult Detail(int? id)
        {
            try
            {
                if (id == null) return NotFound();

                var course = courseRepository.GetCourseByID(id.Value);

                var chapterList = chapterRepository.GetChapters();

                var lessonList = lessonRepository.GetLessons();

                var categoryList = categoryRepository.GetCategorys();

                var instruct = instructRepository.GetInstructs();

                if (course == null) return NotFound();

                return View(Tuple.Create(course, chapterList, categoryList, instruct, lessonList));
            }
            catch (System.Exception)
            {
                return View();
            }
        }

        public ActionResult Create()
        {
            try
            {
                var categoryList = categoryRepository.GetCategorys();

                ViewBag.CategoryList = new SelectList(categoryList, "CategoryId", "CategoryName");

                return View();
            }
            catch (System.Exception)
            {
                return View();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Course course, IFormFile picture)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(course.CourseName))
                    {
                        ModelState.AddModelError("CourseName", "Course name is required.");
                    }

                    if (string.IsNullOrEmpty(course.Description))
                    {
                        ModelState.AddModelError("Description", "Description is required.");
                    }

                    if (picture == null || picture.Length == 0)
                    {
                        ModelState.AddModelError("Picture", "Picture is required.");
                    }

                    if (course.Price == null)
                    {
                        ModelState.AddModelError("Price", "Price is required.");
                    }
                    else if (course.Price < 50)
                    {
                        ModelState.AddModelError("Price", "Price is less 50");
                    }

                    if (ModelState.IsValid)
                    {
                        var urlRelative = "/img/courseImg/";
                        var urlAbsolute = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "courseImg");
                        var fileName = Guid.NewGuid() + Path.GetExtension(picture.FileName);
                        var filePath = Path.Combine(urlAbsolute, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            picture.CopyTo(stream);
                        }

                        course.Picture = Path.Combine(urlRelative, fileName);
                        course.Status = "Wait";
                        course.CreationDate = DateTime.Now;
                        courseRepository.InsertCourse(course);

                        var ins = instructorRepository.GetInstructorByUser(HttpContext.Session.GetString("username"));
                        Instruct instruct = new Instruct { CourseId = course.CourseId, InstructorId = ins.InstructorId };

                        instructRepository.InsertInstruct(instruct);
                        return RedirectToAction("Create", "Chapter", new { courseId = course.CourseId });
                    }
                }
                ViewBag.CategoryId = course.CategoryId;
                var categoryList = categoryRepository.GetCategorys();
                ViewBag.CategoryList = new SelectList(categoryList, "CategoryId", "CategoryName");
                return View(course);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(course);
            }
        }


        public ActionResult Edit(int? id)
        {
            try
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
                var course = courseRepository.GetCourseByID(modelsView.Course.CourseId);
                if (course != null)
                {
                    if (ModelState.IsValid)
                    {
                        course.CreationDate = DateTime.Now;
                        courseRepository.UpdateCourse(modelsView.Course);
                    }
                    return RedirectToAction("Index", "Course", new { id = Request.Cookies["ID"] });
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
            try
            {
                if (id == null) return NotFound();

                var course = courseRepository.GetCourseByID(id.Value);

                if (course == null) return NotFound();

                ViewBag.CategoryId = course.CategoryId;
                var categoryList = categoryRepository.GetCategorys();
                ViewBag.CategoryList = new SelectList(categoryList, "CategoryId", "CategoryName");

                return View(new ModelsView { Course = course });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while processing your request.";
                return View();
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ModelsView modelsView)
        {
            try
            {
                var course = courseRepository.GetCourseByID(modelsView.Course.CourseId);
                if (course != null)
                {
                    courseRepository.DeleteCourse(course.CourseId);
                    return RedirectToAction("Index", new { id = Request.Cookies["ID"] });
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }

        }
    }
}