using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Web.Business; 
using Web.DataAccess; 
using Web.Entity;     
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Web.Controllers
{
    public class WeatherController : Controller
    {
        private readonly WeatherBusinessService _weatherService;
        private readonly AllprojectContext _context;

        public WeatherController(AllprojectContext context)
        {
            _weatherService = new WeatherBusinessService();
            _context = context;
        }

        public IActionResult Manuel() => View();

        [HttpPost]
        public IActionResult Hesapla(string sehir, string sicaklik)
        {
            // 1. Şehir ismini temizle
            string sehirAdi = string.IsNullOrWhiteSpace(sehir) ? "Belirsiz" : sehir;

            double sicaklikDeger = 0;
            if (!string.IsNullOrEmpty(sicaklik))
            {
                string normalizeSicaklik = sicaklik.Replace(",", ".");
                double.TryParse(normalizeSicaklik, NumberStyles.Any, CultureInfo.InvariantCulture, out sicaklikDeger);
            }

            var analizSonucu = _weatherService.AnalizEt(sicaklikDeger);

            try
            {
               
                var userIdStr = HttpContext.Session.GetString("UserId");
                int? currentUserId = !string.IsNullOrEmpty(userIdStr) ? int.Parse(userIdStr) : null;

               
                var city = _context.Cities.FirstOrDefault(c => c.CityName.ToLower() == sehirAdi.ToLower());
                if (city == null)
                {
                    city = new City { CityName = sehirAdi, Country = "Türkiye" };
                    _context.Cities.Add(city);
                    _context.SaveChanges();
                }

               
                int weatherTypeId = sicaklikDeger >= 25 ? 1 : (sicaklikDeger < 10 ? 3 : 2);

                var newLog = new DailyLog
                {
                    CityId = city.CityId,
                    UserId = currentUserId, 
                    Temperature = (decimal)sicaklikDeger,
                    LogDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified), 
                    WeatherTypeId = weatherTypeId,
                    Humidity = 50
                };

                _context.DailyLogs.Add(newLog);
                _context.SaveChanges(); 

                TempData["Basari"] = "Kayıt başarıyla eklendi.";
            }
            catch (Exception ex)
            {
                TempData["Hata"] = "Kayıt Hatası: " + ex.Message;
            }

            TempData["Sehir"] = sehirAdi;
            TempData["Sonuc"] = analizSonucu.Durum;
            TempData["Sicaklik"] = sicaklikDeger.ToString("0.0", CultureInfo.InvariantCulture);
            TempData["Mesaj"] = analizSonucu.Tavsiye;

            return RedirectToAction("SonucGoster");
        }

        public IActionResult SonucGoster()
        {
            ViewBag.Sehir = TempData["Sehir"];
            ViewBag.Durum = TempData["Sonuc"];
            ViewBag.Sicaklik = TempData["Sicaklik"];
            ViewBag.Mesaj = TempData["Mesaj"];
            ViewBag.Hata = TempData["Hata"];
            ViewBag.Basari = TempData["Basari"];
            return View();
        }
    }
}