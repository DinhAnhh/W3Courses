using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebLibrary.Repository;
using WebLibrary.Models;
using WebLibrary.DAO;
using Microsoft.AspNetCore.Http;
using Project_Group3.Models;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Text.RegularExpressions;

namespace Project_Group3.Controllers
{
    public class UserController : Controller
    {
        InstructorRepository instructorRepository = null;
        ILearnerRepository learnerRepository = null;
        IAdminRepository adminRepository = null;
        ISmtpRepository smtpRepository = null;

        private string tmpEmail;
        public UserController()
        {
            learnerRepository = new LearnerRepository();
            instructorRepository = new InstructorRepository();
            adminRepository = new AdminRepository();
            smtpRepository = new StmpRepository();
        }

        public IActionResult Login()
        {
            try
            {
                int? insSession = HttpContext.Session.GetInt32("InsID");
                int? learnerSession = HttpContext.Session.GetInt32("LearnerID");

                Console.WriteLine($"InsID: {insSession}, LearnerID: {learnerSession}");

                if (insSession != null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else if (learnerSession != null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else return View();
            }
            catch (System.Exception)
            {
                return View();
            }
        }


        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid) return View(model);

                var instructor = instructorRepository.GetInstructorByEmailOrUser(model.EmailOrUsername);

                var learner = learnerRepository.GetLearnerByEmailOrUser(model.EmailOrUsername);

                if (instructor != null && instructor.Password == this.GetHashedPassword(model.Password))
                {
                    if (instructor.Status == "Wait")
                    {
                        ViewBag.err = "The account has not been moderated!!!";
                        return View();
                    }
                    else if (instructor.Status == "Delete")
                    {
                        ViewBag.err = "The account no longer exists!!!";
                        return View();
                    }
                    HttpContext.Session.SetInt32("InsID", instructor.InstructorId);
                    HttpContext.Session.SetString("username", instructor.Username);
                    Response.Cookies.Append("Role", "instructor");
                    Response.Cookies.Append("Name", instructor.Username);
                    Response.Cookies.Append("ID", instructor.InstructorId.ToString());
                    return RedirectToAction("Index", "Home");
                }
                else if (learner != null && learner.Password == this.GetHashedPassword(model.Password))
                {
                    HttpContext.Session.SetInt32("LearnerID", learner.LearnerId);
                    HttpContext.Session.SetString("username", learner.Username);
                    Response.Cookies.Append("Role", "learner");
                    Response.Cookies.Append("Name", learner.Username);
                    Response.Cookies.Append("ID", learner.LearnerId.ToString());
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.err = "Email, user or password incorrectly please enter again!.";
                    return View(model);
                }
            }
            catch
            {
                ViewBag.err = "Lỗi đăng nhập!";
                return View();
            }
        }

        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model, IFormFile picture)
        {
            try
            {
                if (!ModelState.IsValid) return View(model);

                DateTime currentDate = DateTime.Now;
                DateTime minimumBirthDate = currentDate.AddYears(-18); // Ngày sinh tối thiểu để đủ 18 tuổi
                var learnerEmail = learnerRepository.GetLearnerByEmail(model.Email);
                var learnerUsername = learnerRepository.GetLearnerByUser(model.Username);
                var instructorEmail = instructorRepository.GetInstructorByEmail(model.Email);
                var instructorUsername = instructorRepository.GetInstructorByUser(model.Username);
                if (learnerEmail != null || instructorEmail != null)
                {
                    ViewBag.err = "This Email account already exists";
                    return View(model);
                }
                if (learnerUsername != null || instructorUsername != null)
                {
                    ViewBag.err = "This Username account already exists";
                    return View(model);
                }
                if (model.Birthday >= minimumBirthDate)
                {
                    ViewBag.err = "Your year of birth is not old enough to register";
                    return View(model);
                }
                if (model.Birthday.Year < 1000)
                {
                    ViewBag.err = "Year of birth must have at least 4 digits";
                    return View(model);
                }
                if (!IsStrongPassword(model.Password))
                {
                    ViewBag.err = "Password must be at least 8 characters long and contain at least one letter and one number.";
                    return View(model);
                }
                if (!IsValidPhoneNumber(model.PhoneNumber))
                {
                    ViewBag.err = "The phone number must be 10 digits long and start with 0.";
                    return View(model);
                }
                if (!IsValidEmail(model.Email))
                {
                    ViewBag.err = "Invalid email address.";
                    return View(model);
                }
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
                    model.Picture = Path.Combine(urlRelative, fileName);
                }

                var LearnerModel = new Learner
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Gender = model.Gender,
                    Birthday = model.Birthday,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    Country = model.Country,
                    Username = model.Username,
                    Password = this.GetHashedPassword(model.Password),
                    RegistrationDate = DateTime.Now.Date,
                    Status = "Active",
                    Picture = model.Picture,
                };
                learnerRepository.InsertLearner(LearnerModel);
                ViewBag.UserId = LearnerModel.LearnerId.ToString();
                ViewBag.Role = "Learner";
                Response.Cookies.Append("MyCookie", LearnerModel.LearnerId.ToString());
                return RedirectToAction("Login", "User");
            }
            catch (Exception ex)
            {
                ViewBag.err = "Đã xảy ra lỗi khi đăng ký: " + ex.Message;
                return View(model);
            }
        }

        public IActionResult InstructorRegister() => View();

        private bool IsStrongPassword(string password)
        {
            return Regex.IsMatch(password, @"^(?=.*[A-Za-z])(?=.*\d).{8,}$");
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber, @"^(0)\d{9}$");
        }
        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@gmail.com$");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InstructorRegister(RegisterViewModel model, IFormFile picture)
        {
            try
            {
                if (!ModelState.IsValid) return View(model);

                if (string.IsNullOrEmpty(model.Introduce))
                {
                    ModelState.AddModelError("InstructorField", "Vui lòng nhập thông tin Instructor");
                    return View(model);
                }

                DateTime currentDate = DateTime.Now;
                DateTime minimumBirthDate = currentDate.AddYears(-21); // Ngày sinh tối thiểu để đủ 21 tuổi
                var learnerEmail = learnerRepository.GetLearnerByEmail(model.Email);
                var learnerUsername = learnerRepository.GetLearnerByUser(model.Username);
                var instructorEmail = instructorRepository.GetInstructorByEmail(model.Email);
                var instructorUsername = instructorRepository.GetInstructorByUser(model.Username);
                if (learnerEmail != null || instructorEmail != null)
                {
                    ViewBag.err = "This Email account already exists";
                    return View(model);
                }
                if (learnerUsername != null || instructorUsername != null)
                {
                    ViewBag.err = "This Username account already exists";
                    return View(model);
                }

                if (model.Birthday >= minimumBirthDate)
                {
                    ViewBag.err = "Your year of birth is not old enough to register";
                    return View(model);
                }
                if (model.Birthday.Year < 1000)
                {
                    ViewBag.err = "Year of birth must have at least 4 digits";
                    return View(model);
                }
                if (!IsStrongPassword(model.Password))
                {
                    ViewBag.err = "Password must be at least 8 characters long and contain at least one letter and one number.";
                    return View(model);
                }
                if (!IsValidPhoneNumber(model.PhoneNumber))
                {
                    ViewBag.err = "The phone number must be 10 digits long and start with 0.";
                    return View(model);
                }
                if (!IsValidEmail(model.Email))
                {
                    ViewBag.err = "Invalid email address.";
                    return View(model);
                }
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
                    model.Picture = Path.Combine(urlRelative, fileName);
                }

                var instructorModel = new Instructor
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Gender = model.Gender,
                    Birthday = model.Birthday,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    Country = model.Country,
                    Username = model.Username,
                    Password = this.GetHashedPassword(model.Password),
                    Picture = model.Picture,
                    RegistrationDate = DateTime.Now.Date,
                    Income = 0,
                    Introduce = model.Introduce,
                    Status = "Wait",
                };

                instructorRepository.InsertInstructor(instructorModel);
                ViewBag.UserId = instructorModel.InstructorId;
                ViewBag.Role = "Learner";
                Response.Cookies.Append("MyCookie", instructorModel.InstructorId.ToString());
                return RedirectToAction("Login", "User");
            }
            catch (Exception ex)
            {
                ViewBag.err = "Đã xảy ra lỗi khi đăng ký: " + ex.Message;
                return View(model);
            }
        }

        public IActionResult Logout()
        {
            try
            {
                foreach (var cookie in HttpContext.Request.Cookies.Keys)
                {
                    Response.Cookies.Delete(cookie);
                }
                HttpContext.Session.Clear();

                return RedirectToAction("Login", "User");
            }
            catch (System.Exception)
            {
                return View();
            }
        }

        public int Random()
        {
            Random random = new Random();
            int otp = random.Next(10000, 99999);

            HttpContext.Session.SetInt32("otp", otp);
            return otp;
        }

        public IActionResult ResetPassword() => View();

        public IActionResult otp(string email)
        {
            try
            {
                int random = Random();

                System.Console.WriteLine(random);

                string tmpEmail = HttpContext.Session.GetString("email");

                if (!string.IsNullOrEmpty(tmpEmail)) smtpRepository.sendMail(tmpEmail, "Mã xác thực OTP", $"Mã xác thực OTP của bạn là: [{random}], tuyệt đối không chia sẻ mã này cho người khác!");

                else return RedirectToAction("CheckEmail");

                return View(email);
            }
            catch (System.Exception)
            {
                return View();
            }
        }


        [HttpPost]
        public IActionResult otp(int otp, int otp1, int otp2, int otp3, int otp4)
        {
            try
            {
                int otpValue = otp * 10000 + otp1 * 1000 + otp2 * 100 + otp3 * 10 + otp4;

                if (HttpContext.Session.GetInt32("otp") == otpValue) return View("ResetPassword");

                return View();
            }
            catch (System.Exception)
            {
                return View();
            }
        }

        public IActionResult CheckEmail() => View();

        [HttpPost]
        public IActionResult CheckEmail(string email)
        {
            try
            {
                var learner = learnerRepository.GetLearnerByEmail(email);
                var instructor = instructorRepository.GetInstructorByEmail(email);

                string UserMail = learner?.Email ?? instructor?.Email ?? "";

                if (UserMail == null) return View();

                HttpContext.Session.SetString("email", UserMail);

                return RedirectToAction("otp");
            }
            catch (System.Exception)
            {
                return View(email);
            }
        }

        [HttpPost]
        public IActionResult ResetPassword(string password, string confirmPassword)
        {
            try
            {
                if (password != confirmPassword) return View(ViewBag.err = "Mk Không khớp");

                string email = HttpContext.Session.GetString("email");

                if (email != null)
                {
                    var learner = learnerRepository.GetLearnerByEmail(email);
                    var instructor = instructorRepository.GetInstructorByEmail(email);
                    if (learner != null)
                    {
                        learnerRepository.UpdatePass(learner.LearnerId, this.GetHashedPassword(password));
                    }

                    if (instructor != null)
                    {
                        instructorRepository.UpdatePass(instructor.InstructorId, this.GetHashedPassword(password));
                    }
                }
                return RedirectToAction("Login");
            }
            catch (System.Exception)
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult ChangePassword(string email, string password, string newPassword, string confirmPassword)
        {
            try
            {

                if (newPassword != confirmPassword) return View();

                if (email != null)
                {
                    var learner = learnerRepository.GetLearnerByEmail(email);
                    var instructor = instructorRepository.GetInstructorByEmail(email);
                    if (learner != null)
                    {
                        learnerRepository.UpdatePass(learner.LearnerId, this.GetHashedPassword(newPassword));
                    }

                    if (instructor != null)
                    {
                        instructorRepository.UpdatePass(instructor.InstructorId, this.GetHashedPassword(newPassword));
                    }
                }

                foreach (var cookie in HttpContext.Request.Cookies.Keys)
                {
                    Response.Cookies.Delete(cookie);
                }

                HttpContext.Session.Clear();
                return RedirectToAction("Login", "User");
            }
            catch (System.Exception)
            {
                return View();
            }
        }

        private string GetHashedPassword(string password)
        {
            try
            {
                using (MD5 md5 = MD5.Create())
                {
                    byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
                    StringBuilder sb = new StringBuilder();

                    for (int i = 0; i < hashBytes.Length; i++)
                    {
                        sb.Append(hashBytes[i].ToString("x2"));
                    }
                    return sb.ToString();
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}