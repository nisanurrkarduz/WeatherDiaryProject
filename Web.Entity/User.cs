using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Entity // DİKKAT: Burası diğer dosyalarla AYNI olmalı
{
    [Table("Users")]
    public partial class User
    {
        public User()
        {
            DailyLogs = new HashSet<DailyLog>();
        }

        [Key]
        [Column("user_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Otomatik artan ID
        public int UserId { get; set; }

        [Column("username")]
        public string Username { get; set; }

        [Column("email")]
        public string Email { get; set; }
        
        [Column("password_hash")]
        public string PasswordHash { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }
        
        [Column("role_id")]
        public int? RoleId { get; set; }

        public virtual Role Role { get; set; }

        // İlişkiler
        public virtual ICollection<DailyLog> DailyLogs { get; set; }
    }
}