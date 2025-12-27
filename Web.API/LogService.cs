namespace Web.API
{
    public class LogService : ILogService
    {
        public string SistemLoguKaydet(string mesaj)
        {
            return $"[SOAP Servisi]: Mesajınız başarıyla kaydedildi -> {mesaj}";
        }
    }
}