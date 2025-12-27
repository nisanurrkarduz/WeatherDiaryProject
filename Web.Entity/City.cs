using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Entity
{
    [Table("Cities")]
    public partial class City
    {
        public City()
        {
            DailyLogs = new HashSet<DailyLog>();
        }

        [Key]
        [Column("city_id")]
        public int CityId { get; set; }

        [Column("city_name")]
        [Required]
        [StringLength(100)]
        public string CityName { get; set; }

        [Column("country")]
        [StringLength(50)]
        public string Country { get; set; }

        [InverseProperty(nameof(DailyLog.City))]
        public virtual ICollection<DailyLog> DailyLogs { get; set; }
    }
}