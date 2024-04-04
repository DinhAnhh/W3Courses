using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Project_Group3.Controllers
{
    public class FileUploadController : Controller
    {
        public IActionResult Index() => View();

        [HttpPost]
        public async Task<IActionResult> UploadFiles(IFormFile file)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (file != null && file.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        ViewBag.FileStatus = "File uploaded successfully.";
                    }
                }
                catch (Exception)
                {
                    ViewBag.FileStatus = "Error while uploading the file.";
                }
            }
            return View();
        }
    }
}