using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Entity
{
    [Table("SystemLogs")]
    public class SystemLog
    {
        [Key]
        [Column("log_id")]
        public int LogId { get; set; }

        [Column("log_date")]
        public DateTime LogDate { get; set; } = DateTime.Now;

        [Column("log_level")] // Info, Error, Warning
        [StringLength(20)]
        public string LogLevel { get; set; }

        [Column("message")]
        public string Message { get; set; }

        [Column("user_id")]
        public int? UserId { get; set; } // Hangi kullanıcı yaptı (opsiyonel)
    }
}
