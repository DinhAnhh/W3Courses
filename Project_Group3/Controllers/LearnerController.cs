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
    public class LearnerController : Controller
    {
        ILearnerRepository learnerRepository = null;

        public LearnerController()
        {
            learnerRepository = new LearnerRepository();
        }

        public IActionResult Index(string search = "", int page = 1, int pageSize = 2)
        {
            var learnerList = learnerRepository.GetLearners();

            if (!string.IsNullOrEmpty(search))
            {
                learnerList = learnerList.Where(i => i.FirstName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 || i.LastName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }

            ViewBag.Search = search;
            var totalCount = learnerList.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            learnerList = learnerList.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.Quantity = totalCount;
            ViewBag.CurrentPage = page;
            return View(learnerList);
        }
        public ActionResult Detail(int? id)
        {
            if (id == null) return NotFound();

            var Learner = learnerRepository.GetLearnerByID(id.Value);

            if (Learner == null) return NotFound();

            return View(Learner);
        }

        public ActionResult Create() => View();
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Learner Learner)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    learnerRepository.InsertLearner(Learner);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(Learner);
            }

        }
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            var learner = learnerRepository.GetLearnerByID(id.Value);

            if (learner == null) return NotFound();

            return View(learner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Learner learner)
        {
            try
            {
                if (id != learner.LearnerId) return NotFound();

                if (ModelState.IsValid)
                {
                    learnerRepository.UpdateLearner(learner);
                }
                return RedirectToAction("Learner", "Admin");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(int id, Learner learner)
        {
            try
            {
                var existingLearner = learnerRepository.GetLearnerByID(id);
                if (existingLearner == null) return NotFound();

                if (ModelState.IsValid)
                {
                    learnerRepository.UpdateLearner(learner);
                    return RedirectToAction("Index", "Home");
                }
                return View(learner);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(learner);
            }
        }

        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            var learner = learnerRepository.GetLearnerByID(id.Value);
            
            if (learner == null) return NotFound();

            return View(learner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                learnerRepository.DeleteLearner(id);
                return RedirectToAction("Learner", "Admin");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }
    }
}