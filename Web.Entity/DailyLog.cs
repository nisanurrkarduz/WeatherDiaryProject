using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Entity
{
    [Table("DailyLogs")]
    public partial class DailyLog
    {
        [Key]
        [Column("log_id")]
        public int LogId { get; set; }

        [Column("city_id")]
        public int? CityId { get; set; }

        [Column("user_id")]
        public int? UserId { get; set; }

        [Column("log_date", TypeName = "timestamp without time zone")]
        public DateTime? LogDate { get; set; }

        [Column("temperature")]
        public decimal Temperature { get; set; }

        [Column("humidity")]
        public int? Humidity { get; set; }

        [Column("weather_type_id")]
        public int? WeatherTypeId { get; set; }

        [ForeignKey(nameof(CityId))]
        [InverseProperty("DailyLogs")]
        public virtual City City { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("DailyLogs")]
        public virtual User User { get; set; }

        [ForeignKey(nameof(WeatherTypeId))]
        [InverseProperty("DailyLogs")]
        public virtual WeatherType WeatherType { get; set; }
    }
}