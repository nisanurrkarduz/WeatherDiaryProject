using System;
using System.ComponentModel.DataAnnotations;


namespace Web.Entity
{
    public class HavaKayit
    {
        [Key]
        public int Id { get; set; }
        public string Sehir { get; set; }
        public double Sicaklik { get; set; }
        public string DuyguDurumu { get; set; } // Mutlu, Stresli vs.
        public DateTime Tarih { get; set; } = DateTime.Now;
        public string KaydedenKullanici { get; set; } // Rol yönetimi için
    }
}