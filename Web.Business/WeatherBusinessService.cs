using System;

namespace Web.Business
{
    // İSTER: 6 Katmanlı SOA Tasarımı - Business Katmanı
    public class WeatherBusinessService
    {
        public MoodResult AnalizEt(double sicaklik)
        {
            string durum;
            string tavsiye;

            if (sicaklik > 25)
            {
                durum = "Mutlu";
                tavsiye = "Hava harika! Dışarı çıkıp güneşin tadını çıkarabilirsin.";
            }
            else if (sicaklik < 15)
            {
                durum = "Stresli";
                tavsiye = "Hava biraz soğuk, iç mekanlarda vakit geçirmek sana iyi gelebilir.";
            }
            else
            {
                durum = "Nötr";
                tavsiye = "Sakin bir hava var, rutin işlerine odaklanabilirsin.";
            }

            return new MoodResult
            {
                Durum = durum,
                Tavsiye = tavsiye,
                Sicaklik = sicaklik
            };
        }
    }

    public class MoodResult
    {
        public string Durum { get; set; }
        public string Tavsiye { get; set; }
        public double Sicaklik { get; set; }
    }
}