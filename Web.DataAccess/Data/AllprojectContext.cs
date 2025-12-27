using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Web.Entity; // KRİTİK: Modeller artık Web.Entity içinde, burayı böyle güncelle!

namespace Web.DataAccess // BURASI TAM OLMALI: Katman adınla aynı yap
{
    public partial class AllprojectContext : DbContext
    {
        public AllprojectContext()
        {
        }

        public AllprojectContext(DbContextOptions<AllprojectContext> options)
            : base(options)
        {
        }

        // Tablolar (Web.Entity içinden okunuyor)
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<DailyLog> DailyLogs { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<WeatherType> WeatherTypes { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<SystemLog> SystemLogs { get; set; }
        public virtual DbSet<UserLogDetail> UserLogDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserLogDetail>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("View_UserLogDetails");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}