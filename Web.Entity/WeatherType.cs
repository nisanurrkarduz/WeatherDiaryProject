using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Entity // DİKKAT: Namespace aynı olmalı
{
    [Table("WeatherTypes")]
    public partial class WeatherType
    {
        public WeatherType()
        {
            DailyLogs = new HashSet<DailyLog>();
        }

        [Key]
        [Column("type_id")]
        public int TypeId { get; set; }

        [Column("type_name")]
        public string TypeName { get; set; }

        [Column("risk_level")]
        public int? RiskLevel { get; set; }

        // İlişkiler
        public virtual ICollection<DailyLog> DailyLogs { get; set; }
    }
}