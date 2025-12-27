using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Entity;
using Web.DataAccess;
using System.Linq;

namespace DuyguAnaliziWeb.Controllers
{
    
    public class ProfileController : Controller
    {
        private readonly AllprojectContext _context;

        public ProfileController(AllprojectContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Kullanıcı giriş yapmış mı?
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = int.Parse(userIdStr);

            // Kullanıcı bilgilerini ve geçmiş loglarını getir
            var user = _context.Users
                        .Include(u => u.Role)
                        .Include(u => u.DailyLogs)
                            .ThenInclude(d => d.City)
                        .Include(u => u.DailyLogs)
                            .ThenInclude(d => d.WeatherType)
                        .FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(user);
        }
    }
}
