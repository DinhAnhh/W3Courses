using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Project_Group3.Models;
using WebLibrary.Models;
using WebLibrary.Repository;
namespace Project_Group3.Controllers
{

    public class ChapterController : Controller
    {

        IChapterRepository chapterRepository = null;
        ICourseRepository courseRepository = null;
        ICategoryRepository categoryRepository = null;
        ILessonRepository lessonRepository = null;
        IInstructorRepository instructorRepository = null;
        public ChapterController()
        {
            courseRepository = new CourseRepository();
            categoryRepository = new CategoryRepository();
            chapterRepository = new ChapterRepository();
            lessonRepository = new LessonRepository();
            instructorRepository = new InstructorRepository();
        }

        public ActionResult Index(int courseId, string search = "")
        {
            try
            {
                ViewBag.CourseId = courseId;

                var course = courseRepository.GetCourseByID(courseId);

                if (course == null) return NotFound();

                ViewBag.CourseName = course.CourseName;

                var chapterList = chapterRepository.GetChapters();

                var chaptersToDisplay = chapterList.Where(c => c.CourseId == courseId);

                if (!string.IsNullOrEmpty(search))
                {
                    chaptersToDisplay = chaptersToDisplay.Where(c => c.ChapterName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0);
                }

                chaptersToDisplay = chaptersToDisplay.OrderBy(c => c.Index);

                var modelsView = new ModelsView
                {
                    ChaptersList = chaptersToDisplay.ToList(),
                    LessonsList = lessonRepository.GetLessons().ToList(),
                };

                ViewBag.Search = search;

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

                var Chapter = chapterRepository.GetChapterByID(id.Value);

                if (Chapter == null) return NotFound();

                return View(Chapter);
            }
            catch (System.Exception)
            {
                return View(id);
            }
        }

        public ActionResult Create(int courseId)
        {
            var course = courseRepository.GetCourseByID(courseId);
            ViewBag.CourseId = courseId;

            if (course == null)
            {
                ViewBag.CourseName = "Unknown Course";
            }
            else
            {
                ViewBag.CourseName = string.IsNullOrEmpty(course.CourseName) ? "Unknown Course" : course.CourseName;
            }

            // Lấy danh sách các index của chương đã tồn tại cho khóa học này
            var existingChapters = chapterRepository.GetChapters()
             .Where(c => c.CourseId == courseId)
             .Select(c => c.Index)
             .OrderBy(index => index) // Sắp xếp theo thứ tự tăng dần index
             .ToList();
            ViewBag.ExistingChapterIndexes = existingChapters;

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Chapter chapter, bool redirectToCreateLesson, int courseId)
        {
            try
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        if (!string.IsNullOrEmpty(chapter.ChapterName))
                        {
                            if (chapter.Index.HasValue && chapter.Index.Value > 0)
                            {
                                var existingChapter = chapterRepository.GetChapters().FirstOrDefault(c => c.Index == chapter.Index && c.CourseId == courseId);
                                if (existingChapter != null)
                                {
                                    ModelState.AddModelError("Index", "An existing chapter with the same index already exists for this course.");
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(chapter.Description))
                                    {
                                        chapterRepository.InsertChapter(chapter);

                                        if (redirectToCreateLesson)
                                        {
                                            return RedirectToAction("Create", "Lesson", new { chapterId = chapter.ChapterId, courseId = chapter.CourseId });
                                        }
                                        else
                                        {
                                            return RedirectToAction("Index", new { courseId = chapter.CourseId });
                                        }
                                    }
                                    else
                                    {
                                        ModelState.AddModelError("Description", "Description is required.");
                                    }
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("Index", "Index must be a positive value.");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("ChapterName", "ChapterName is required.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;
                    return View();
                }

                ViewBag.CourseId = courseId;
                var course = courseRepository.GetCourseByID(courseId);
                ViewBag.CourseName = course.CourseName;
                return View(chapter);
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

                var Chapter = chapterRepository.GetChapterByID(id.Value);

                if (Chapter == null) return NotFound();

                ModelsView modelsView = new ModelsView { Chapter = Chapter };

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
                if (ModelState.IsValid)
                {
                    var chapter = chapterRepository.GetChapterByID(modelsView.Chapter.ChapterId);

                    if (chapter != null)
                    {
                        chapterRepository.UpdateChapter(modelsView.Chapter);
                        return RedirectToAction("Index", new { courseId = chapter.CourseId });
                    }
                    else
                    {
                        ViewBag.Message = "Chapter not found.";
                        return View(modelsView);
                    }
                }
                else
                {
                    ViewBag.Message = "Model state is invalid.";
                    return View(modelsView);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        public IActionResult LearnerChapter(int courseId)
        {
            try
            {
                var course = courseRepository.GetCourseByID(courseId);
                var chapterlist = chapterRepository.GetChapters();
                var lessonlist = lessonRepository.GetLessons();
                ModelsView modelsView = new ModelsView
                {
                    Course = course,
                    ChaptersList = chapterlist.ToList(),
                    LessonsList = lessonlist.ToList(),
                };
                return View(modelsView);
            }
            catch (System.Exception)
            {
                return View();
            }
        }

        public IActionResult Delete(int? id)
        {
            try
            {
                var chapter = chapterRepository.GetChapterByID(id.Value);

                if (chapter == null) return NotFound();

                var model = new ModelsView
                {
                    Chapter = chapter
                };

                return View(model);
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
                var chapter = chapterRepository.GetChapterByID(modelsView.Chapter.ChapterId);
                if (chapter != null)
                {
                    System.Console.WriteLine(chapter.ChapterId);
                    chapterRepository.DeleteChapter(chapter.ChapterId);
                    return RedirectToAction("Index", new { courseId = modelsView.Chapter.CourseId });
                }
                return View(modelsView);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }
    }
}