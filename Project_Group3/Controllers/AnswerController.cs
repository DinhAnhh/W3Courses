using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebLibrary.Models;
using WebLibrary.Repository;

namespace Project_Group3.Controllers
{

    public class AnswerController : Controller
    {
        private readonly IAnswerRepository answerRepository;

        public AnswerController()
        {
            answerRepository = new AnswerRepository();
        }



        public ActionResult Detail(int? id)
        {
            try
            {
                if (id == null) return NotFound();

                var answer = answerRepository.GetAnswerByID(id.Value);

                if (answer == null) return NotFound();

                return View(answer);
            }
            catch (System.Exception)
            {
                return View();
            }

        }

        public ActionResult Index(int courseId)
        {
            try
            {
                ViewBag.CourseId = courseId;
                var answerList = answerRepository.GetAnswers();
                return View(answerList);
            }
            catch (System.Exception)
            {
                return View(courseId);
            }
        }

        public ActionResult Create(int courseId)
        {
            try
            {
                ViewBag.CourseId = courseId;
                return View();
            }
            catch (System.Exception)
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Answer answer, int courseId)
        {
            ViewBag.CourseId = courseId;
            try
            {
                answerRepository.InsertAnswer(answer);

                return RedirectToAction("Index", new { courseId = ViewBag.CourseId });
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(answer);
            }
        }

        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null) return NotFound();

                var answer = answerRepository.GetAnswerByID(id.Value);

                if (answer == null) return NotFound();

                return View(answer);
            }
            catch (System.Exception)
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Answer answer)
        {
            try
            {
                if (id != answer.AnswerId) return NotFound();

                if (ModelState.IsValid) answerRepository.UpdateAnswer(answer);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }
        public ActionResult Delete(int? id, int courseId)
        {
            try
            {
                if (id == null) return NotFound();

                ViewBag.CourseId = courseId;
                var answer = answerRepository.GetAnswerByID(id.Value);

                if (answer == null) return NotFound();

                return View(answer);
            }
            catch (System.Exception)
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, int courseId)
        {
            ViewBag.CourseId = courseId;
            try
            {
                answerRepository.DeleteAnswer(id);
                return RedirectToAction("Index", "Answer", new { courseId = ViewBag.CourseId });
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                var answer = answerRepository.GetAnswerByID(id);
                return View(answer);
            }
        }
    }
}