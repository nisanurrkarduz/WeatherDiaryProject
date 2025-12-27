using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Web.GrpcService; 
using System.Net.Http;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AdviceService.AdviceServiceClient _adviceClient;

        public HomeController(IHttpClientFactory httpClientFactory, AdviceService.AdviceServiceClient adviceClient)
        {
            _httpClientFactory = httpClientFactory;
            _adviceClient = adviceClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index(double? lat, double? lng, string? city)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(city))
                    ViewBag.SecilenSehir = city;

                if (lat.HasValue && lng.HasValue)
                    ViewBag.SecilenKonum = $"{lat.Value:F5}, {lng.Value:F5}";

                var http = _httpClientFactory.CreateClient();

                
                var nodeUrl = "http://localhost:3000/tahmin";
                if (lat.HasValue && lng.HasValue)
                {
                    nodeUrl = $"http://localhost:3000/tahmin?lat={lat.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)}&lng={lng.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)}";
                }

                // Node.js'ten hava  durum verisi al
                var nodeJson = await http.GetStringAsync(nodeUrl);

                var node = JsonSerializer.Deserialize<NodeTahminResponse>(
                    nodeJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                if (node == null)
                {
                    ViewBag.Hata = "Node.js servisinden veri alınamadı (JSON boş döndü).";
                    return View();
                }

                ViewBag.Durum = node.Durum;
                ViewBag.Sicaklik = node.Sicaklik;
                ViewBag.Mesaj = node.Mesaj;

              // gRPC'den tavsiye al
                var reply = await _adviceClient.GetAdviceAsync(new AdviceRequest
                {
                    Mood = node.Durum ?? "Nötr",
                    Temperature = node.Sicaklik,
                    Precipitation = node.Yagis
                });

                ViewBag.Tavsiye = reply?.Advice ?? "Tavsiye üretilemedi.";

                return View();
            }
            catch (HttpRequestException ex)
            {
                ViewBag.Hata = $"Node.js Servisine Bağlanılamadı! (Lütfen NodeService projesini çalıştırın). Hata: {ex.Message}";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Hata = $"Beklenmeyen bir hata oluştu: {ex.Message}";
                return View();
            }
        }

        
        public IActionResult Privacy()
        {
            return View();
        }

        // Node.js'ten gelen JSON yapısı için model
        private class NodeTahminResponse
        {
            public string Durum { get; set; }
            public double Sicaklik { get; set; }
            public double Yagis { get; set; }
            public string Mesaj { get; set; }
            public string Kaynak { get; set; }
        }
    }
}