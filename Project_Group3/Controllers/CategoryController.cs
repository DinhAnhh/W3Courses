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

    public class CategoryController : Controller
    {

        ICategoryRepository categoryRepository = null;
        public CategoryController() => categoryRepository = new CategoryRepository();
        //Get LearnerController
        public ActionResult Index()
        {
            var Categorylist = categoryRepository.GetCategorys();
            return View(Categorylist);
        }
        public ActionResult Detail(int? id)
        {
            try
            {
                if (id == null) return NotFound();

                var Category = categoryRepository.GetCategoryByID(id.Value);

                if (Category == null) return NotFound();

                return View(Category);
            }
            catch (System.Exception)
            {
                return View();
            }
        }

        public ActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    categoryRepository.InsertCategory(category);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(category);
            }

        }

        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null) return NotFound();

                var Category = categoryRepository.GetCategoryByID(id.Value);

                if (Category == null) return NotFound();

                return View(Category);
            }
            catch (System.Exception)
            {
                return View(id);
            }

        }
        //Post  Learnercontroller/edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Category Category)
        {
            try
            {
                if (id != Category.CategoryId) return NotFound();

                if (ModelState.IsValid)
                {
                    categoryRepository.UpdateCategory(Category);
                }
                return RedirectToAction(nameof(Index));
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

                var category = categoryRepository.GetCategoryByID(id.Value);

                if (category == null) return NotFound();

                return View(category);
            }
            catch (System.Exception)
            {
                return View(id);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                categoryRepository.DeleteCategory(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = "An error occurred while deleting the category.";
                ViewBag.ErrorMessage = ex.Message;
                ViewBag.StackTrace = ex.StackTrace;
                ViewBag.InnerErrorMessage = ex.InnerException?.Message;
                ViewBag.InnerStackTrace = ex.InnerException?.StackTrace;

                var category = categoryRepository.GetCategoryByID(id);
                return View(category);
            }
        }
    }
}