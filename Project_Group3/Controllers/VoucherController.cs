using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Project_Group3.Models;
using WebLibrary.DAO;
using WebLibrary.Models;
using WebLibrary.Repository;

namespace Project_Group3.Controllers
{
    public class VoucherController : Controller
    {
        IVoucherRepository voucherRepository = null;

        public VoucherController()
        {
            voucherRepository = new VoucherRepository();
        }

        public IActionResult Index()
        {
            try
            {
                var list = voucherRepository.VouchersList();
                return View(list);
            }
            catch (System.Exception)
            {
                return View();
            }
        }

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Voucher voucher)
        {
            try
            {
                if (voucher == null)
                {
                    ViewBag.err = "voucher cannot be null. Please try again.";
                    return View();
                }

                if (ModelState.IsValid)
                {
                    var generatedCode = voucherRepository.GenerateRandomCode();
                    voucher = new Voucher
                    {
                        CodeVoucher = generatedCode,
                        AdminId = 1,
                        PercentDiscount = voucher.PercentDiscount,
                        EndAt = voucher.EndAt,
                        StartAt = voucher.StartAt,
                        IsActive = voucher.IsActive
                    };
                    string voucherInfo = $"Voucher ID: {voucher.VoucherId}\n" +
                                         $"Code Voucher: {voucher.CodeVoucher}\n" +
                                         $"Admin ID: {voucher.AdminId}\n" +
                                         $"Percent Discount: {voucher.PercentDiscount}\n" +
                                         $"End Date: {voucher.EndAt}\n" +
                                         $"Start Date: {voucher.StartAt}\n" +
                                         $"Is Active: {voucher.IsActive}";
                    Console.WriteLine(voucherInfo);
                    voucherRepository.CreateVoucher(voucher);
                    return RedirectToAction("Index");
                }
                else
                {
                    var errorProperties = ModelState.Keys.Where(k => ModelState[k].Errors.Any()).ToList();
                    ViewBag.ErrorProperties = errorProperties;
                    return View(voucher);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(voucher);
            }
        }

        public IActionResult Delete(int? id)
        {
            try
            {
                if (id == null) return NotFound();

                var voucher = voucherRepository.GetVoucherByID(id.Value);

                if (voucher == null)
                {
                    return NotFound();
                }

                return View(voucher);
            }
            catch (System.Exception)
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                voucherRepository.Remove(id);
                TempData["DeleteSuccess"] = true;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }
    }
}