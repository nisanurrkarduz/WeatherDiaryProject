using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Entity
{
    [Table("Roles")]
    public class Role
    {
        [Key]
        [Column("role_id")]
        public int RoleId { get; set; }

        [Column("role_name")]
        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }

        // İlişki
        public virtual ICollection<User> Users { get; set; }
    }
}
