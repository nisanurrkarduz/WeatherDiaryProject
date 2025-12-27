using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using Web.Entity;     
using Web.DataAccess;  

namespace Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly AllprojectContext _context;

        public AccountController(AllprojectContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // Kullanıcıyı veritabanından, Rolü ile birlikte getiriyoruz
            var user = _context.Users
                        .Include(u => u.Role)
                        .FirstOrDefault(u => u.Username == username && u.PasswordHash == password);

            if (user != null)
            {
                
                HttpContext.Session.SetString("UserId", user.UserId.ToString());
                HttpContext.Session.SetString("Username", user.Username);

                string roleName = user.Role != null ? user.Role.RoleName : "User";
                HttpContext.Session.SetString("UserRole", roleName);

               //giriş logunu oluşturuyoruz
                var log = new SystemLog
                {
                    LogDate = DateTime.Now,
                    LogLevel = "Info",
                    Message = $"{username} giriş yaptı.",
                    UserId = user.UserId
                };
                _context.SystemLogs.Add(log);
                _context.SaveChanges();

                if (roleName == "Admin")
                    return RedirectToAction("Index", "Admin");
                else
                    return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Kullanıcı adı veya şifre hatalı!";
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User model, string confirmPassword)
        {
            // Validasyonlar
            if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.PasswordHash))
            {
                ViewBag.Error = "Kullanıcı adı ve şifre zorunludur.";
                return View(model);
            }

            if (model.PasswordHash != confirmPassword)
            {
                ViewBag.Error = "Şifreler uyuşmuyor.";
                return View(model);
            }

            if (_context.Users.Any(u => u.Username == model.Username))
            {
                ViewBag.Error = "Bu kullanıcı adı zaten alınmış.";
                return View(model);
            }

            // yeni kullanıcıyı oluşturma 
            model.RoleId = 2;
            model.CreatedAt = DateTime.Now;

            _context.Users.Add(model);
            _context.SaveChanges();

            // yeni kayıt için sistem logu
            var log = new SystemLog
            {
                LogDate = DateTime.Now,
                LogLevel = "Info",
                Message = $"Yeni kayıt: {model.Username}",
                UserId = model.UserId
            };
            _context.SystemLogs.Add(log);
            _context.SaveChanges();

            TempData["Success"] = "Kayıt başarılı! Lütfen giriş yapın.";
            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (!string.IsNullOrEmpty(userIdStr) && int.TryParse(userIdStr, out int userId))
            {
                var log = new SystemLog
                {
                    LogDate = DateTime.Now,
                    LogLevel = "Info",
                    Message = $"{HttpContext.Session.GetString("Username")} çıkış yaptı.",
                    UserId = userId
                };
                _context.SystemLogs.Add(log);
                _context.SaveChanges();
            }

            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}