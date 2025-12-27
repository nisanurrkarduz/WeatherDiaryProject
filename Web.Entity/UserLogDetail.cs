using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Entity
{
    // Veritabanı View karşılığı (Keyless Entity)
    public class UserLogDetail
    {
        public string Username { get; set; }
        public string CityName { get; set; }
        public decimal Temperature { get; set; }
        public DateTime? LogDate { get; set; }
        public string WeatherType { get; set; }
    }
}
