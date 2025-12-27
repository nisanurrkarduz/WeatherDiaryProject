using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Entity;      
using Web.DataAccess;  
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Controllers; 
public class AdminController : Controller
{
    private readonly AllprojectContext _context;

    public AdminController(AllprojectContext context)
    {
        _context = context;
    }

    // Listeleme Sayfası
    public IActionResult Index()
    {
        var role = HttpContext.Session.GetString("UserRole");
        if (role != "Admin")
        {
            return RedirectToAction("Login", "Account");
        }

        var logs = _context.DailyLogs
                    .Include(l => l.City)
                    .Include(l => l.User)
                    .Include(l => l.WeatherType)
                    .OrderByDescending(l => l.LogDate)
                    .ToList();

        return View(logs);
    }

    // --- YENİ KAYIT (CREATE) ---

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.CityId = new SelectList(_context.Cities, "CityId", "CityName");
        ViewBag.WeatherTypeId = new SelectList(_context.WeatherTypes, "TypeId", "TypeName");
        ViewBag.UserId = new SelectList(_context.Users, "UserId", "Username");
        
        return View();
    }

    [HttpPost]
    public IActionResult Create(DailyLog log)
    {
        if (ModelState.IsValid)
        {
            if (log.LogDate == null) log.LogDate = DateTime.Now;

            _context.DailyLogs.Add(log);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        ViewBag.CityId = new SelectList(_context.Cities, "CityId", "CityName", log.CityId);
        ViewBag.WeatherTypeId = new SelectList(_context.WeatherTypes, "TypeId", "TypeName", log.WeatherTypeId);
        ViewBag.UserId = new SelectList(_context.Users, "UserId", "Username", log.UserId);
        
        return View(log);
    }

    // --- DÜZENLEME (EDIT) ---

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var log = _context.DailyLogs.Find(id);
        if (log == null) return NotFound();

        ViewBag.Cities = _context.Cities.ToList();
        ViewBag.WeatherTypes = _context.WeatherTypes.ToList();
        ViewBag.Users = _context.Users.ToList();

        return View(log);
    }

    [HttpPost]
    public IActionResult Edit(DailyLog log)
    {
        if (ModelState.IsValid)
        {
            _context.DailyLogs.Update(log);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        
        ViewBag.Cities = _context.Cities.ToList();
        ViewBag.WeatherTypes = _context.WeatherTypes.ToList();
        ViewBag.Users = _context.Users.ToList();
        return View(log);
    }

    // --- SİLME (DELETE) ---

    public IActionResult Delete(int id)
    {
        var log = _context.DailyLogs.Find(id);
        if (log != null)
        {
            _context.DailyLogs.Remove(log);
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }

    // --- RAPORLAR VE SP ---

    public IActionResult Raporlar()
    {
        var rapor = _context.UserLogDetails.ToList();
        return View(rapor);
    }

    public IActionResult YuksekSicakliklar()
    {
        var highTemps = _context.Database
            .SqlQueryRaw<HighTempViewModel>("SELECT * FROM sp_GetHighTempLogs({0})", 20.0m) 
            .ToList();

        return View(highTemps);
    }
}

// SP Dönüş Modeli
public class HighTempViewModel
{
    public int LogId { get; set; }
    public string CityName { get; set; }
    public decimal Temperature { get; set; }
}