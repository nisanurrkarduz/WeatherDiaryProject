using Microsoft.AspNetCore.Mvc;
using Web.Business;
using Web.Core;

namespace Web.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherApiController : ControllerBase
    {
        private readonly WeatherBusinessService _businessService;

        public WeatherApiController()
        {
            _businessService = new WeatherBusinessService();
        }

        [HttpGet("analiz")]
        public IActionResult GetHavaAnalizi(double sicaklik)
        {
            // Business katmanını çağırıp sonucu Core modeline sarmalıyoruz
            var analiz = _businessService.AnalizEt(sicaklik);
            return Ok(ServiceResult<MoodResult>.Success(analiz));
        }
    }
}