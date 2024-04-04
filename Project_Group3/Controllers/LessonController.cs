using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Project_Group3.Models;
using WebLibrary.Models;
using WebLibrary.Repository;
namespace Project_Group3.Controllers
{

    public class LessonController : Controller
    {

        ILessonRepository lessonRepository = null;
        IChapterRepository chapterRepository = null;
        ICourseRepository courseRepository = null;
        public LessonController()
        {
            lessonRepository = new LessonRepository();
            chapterRepository = new ChapterRepository();
            courseRepository = new CourseRepository();
        }

        public ActionResult Index(int chapterId, int courseId, string search = "")
        {
            try
            {
                var chapter = chapterRepository.GetChapterByID(chapterId);
                var course = courseRepository.GetCourseByID(courseId);
                ViewBag.CourseId = courseId;
                ViewBag.ChapterId = chapterId;

                ViewBag.ChapterName = chapter.ChapterName;
                ViewBag.CourseName = course.CourseName;

                var lessonList = lessonRepository.GetLessons();

                var lessonsToDisplay = lessonList.Where(l => l.ChapterId == chapterId && l.CourseId == courseId).ToList();

                if (!string.IsNullOrEmpty(search))
                {
                    lessonsToDisplay = lessonsToDisplay.Where(l => l.LessonName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                }

                lessonsToDisplay = lessonsToDisplay.OrderBy(l => l.Index).ToList();

                ViewBag.Search = search;

                return View(lessonsToDisplay);
            }
            catch (System.Exception)
            {
                return View();
            }

        }
        public ActionResult Detail(int? id)
        {
            try
            {
                if (id == null) return NotFound();

                var Lesson = lessonRepository.GetLessonByID(id.Value);

                if (Lesson == null) return NotFound();

                return View(Lesson);
            }
            catch (System.Exception)
            {
                return View();
            }
        }

        public ActionResult Create(int chapterId, int courseId)
        {
            var chapter = chapterRepository.GetChapterByID(chapterId);
            var course = courseRepository.GetCourseByID(courseId);
            ViewBag.ChapterId = chapterId;
            ViewBag.CourseId = courseId;

            ViewBag.ChapterName = chapter.ChapterName;
            ViewBag.CourseName = course.CourseName;
            // Lấy danh sách các index của existing lessons từ repository hoặc nguồn dữ liệu tương ứng.
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Lesson lesson, int chapterId, int courseId, IFormFile video)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Kiểm tra điều kiện cho thuộc tính LessonName
                    if (!string.IsNullOrEmpty(lesson.LessonName))
                    {
                        // Kiểm tra điều kiện cho thuộc tính Description
                        if (!string.IsNullOrEmpty(lesson.Description))
                        {
                            // Kiểm tra điều kiện cho thuộc tính Index
                            if (lesson.Index.HasValue && lesson.Index.Value > 0)
                            {
                                var existingLesson = lessonRepository.GetLessons()
    .FirstOrDefault(l => l.Index == lesson.Index && l.CourseId == courseId);

                                if (existingLesson != null)
                                {
                                    ModelState.AddModelError("Index", "Index must be unique within the course.");
                                }
                                if (video == null || video.Length == 0)
                                {
                                    ModelState.AddModelError("Video", "Video is required.");
                                }

                                if (ModelState.IsValid)
                                {
                                    // Handle the video file
                                    var urlRelative = "/video/";
                                    var urlAbsolute = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "video");
                                    var fileName = Guid.NewGuid() + Path.GetExtension(video.FileName);
                                    var filePath = Path.Combine(urlAbsolute, fileName);

                                    using (var stream = new FileStream(filePath, FileMode.Create))
                                    {
                                        video.CopyTo(stream);
                                    }

                                    lesson.Content = Path.Combine(urlRelative, fileName);
                                    lessonRepository.InsertLesson(lesson);

                                    return RedirectToAction("Index", new { chapterId = chapterId, courseId = courseId });
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("Index", "Index must be a positive value.");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("Description", "Description is required.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("LessonName", "LessonName is required.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                ViewBag.Message = ex.Message;
            }
            var chapter = chapterRepository.GetChapterByID(chapterId);
            ViewBag.ChapterId = chapterId;
            ViewBag.CourseId = courseId;
            ViewBag.ChapterName = chapter.ChapterName;
            return View(lesson);
        }
        public IActionResult LearnerLesson(int courseId, int chapterId)
        {
            try
            {
                var course = courseRepository.GetCourseByID(courseId);
                var chapter = chapterRepository.GetChapterByID(chapterId);
                var lessonlist = lessonRepository.GetLessons();
                ModelsView modelsView = new ModelsView
                {
                    Course = course,
                    Chapter = chapter,
                    LessonsList = lessonlist.ToList(),
                };
                return View(modelsView);
            }
            catch (System.Exception)
            {
                return View();
            }
        }

        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null) return NotFound();

                var Lesson = lessonRepository.GetLessonByID(id.Value);

                if (Lesson == null) return NotFound();

                return View(new ModelsView { Lesson = Lesson });
            }
            catch (System.Exception)
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ModelsView modelsView, IFormFile video)
        {
            try
            {
                var lesson = lessonRepository.GetLessonByID(modelsView.Lesson.LessonId);
                if (lesson != null)
                {
                    if (video != null && video.Length > 0)
                    {
                        var urlRelative = "/video/";
                        var urlAbsolute = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "video");
                        var fileName = Guid.NewGuid() + Path.GetExtension(video.FileName);
                        var filePath = Path.Combine(urlAbsolute, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            video.CopyTo(stream);
                        }

                        lesson.Content = Path.Combine(urlRelative, fileName);
                    }

                    lesson.LessonName = modelsView.Lesson.LessonName;
                    lesson.Description = modelsView.Lesson.Description;
                    lesson.Content = modelsView.Lesson.Content;
                    lesson.Index = modelsView.Lesson.Index;

                    lessonRepository.UpdateLesson(lesson);
                    return RedirectToAction("Index", new { chapterId = lesson.ChapterId, courseId = lesson.CourseId });
                }
                return View(modelsView);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(modelsView);
            }
        }

        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null) return NotFound();

                var Lesson = lessonRepository.GetLessonByID(id.Value);

                if (Lesson == null) return NotFound();

                return View(new ModelsView { Lesson = Lesson });
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
                var lesson = lessonRepository.GetLessonByID(modelsView.Lesson.LessonId);
                if (lesson != null)
                {
                    lessonRepository.DeleteLesson(lesson.LessonId);
                    return RedirectToAction("Index", new { chapterId = lesson.ChapterId, courseId = lesson.CourseId });
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