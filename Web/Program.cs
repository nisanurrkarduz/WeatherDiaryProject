using Microsoft.EntityFrameworkCore;
using Web.Entity;         // Modellerin bulunduğu katman
using Web.DataAccess;     // Veritabanı ve Context katmanı
using SoapCore;           // SOAP protokolü desteği
using Web.API;            // ILogService ve LogService'in bulunduğu katman

// PostgreSQL tarih (DateTime) hatalarını önlemek için kritik ayar
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// --- SERVİSLERİN KAYDEDİLMESİ (DEPENDENCY INJECTION) ---
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();           // Node.js ve Hazır API ile konuşmak için
builder.Services.AddSession();              // Oturum/Rol yönetimi için
builder.Services.AddHttpContextAccessor();  // Session'a her yerden ulaşmak için

builder.Services.AddGrpcClient<Web.GrpcService.AdviceService.AdviceServiceClient>(o =>
{
    o.Address = new Uri("http://localhost:7001");
});
// İSTER 2: SOAP İletişim Protokolü (20 Puan)
// SOAP Servislerini sisteme tanıtıyoruz
builder.Services.AddSoapCore();
builder.Services.AddScoped<ILogService, LogService>();

// --- VERİTABANI BAĞLANTISI ---
// appsettings.json dosyasındaki "DefaultConnection" adresini kullanır
builder.Services.AddDbContext<AllprojectContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// --- VERİTABANI BAŞLATMA (SEED DATA) ---
// Uygulama her çalıştığında veritabanı kontrol edilir ve örnek veriler yüklenir
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AllprojectContext>();
        // Web.DataAccess katmanındaki Initializer'ı çağırıyoruz
        Web.DataAccess.DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Veritabanı başlatılırken hata oluştu.");
    }
}

// HTTP İSTEK KANALI (MIDDLEWARE) AYARLARI
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// DİKKAT: Burada UserRouting yazıyordu, UseRouting olarak düzeltildi.
app.UseRouting();

app.UseAuthorization();
app.UseSession(); // Session'ı aktif et (Authentication'dan sonra gelmeli)

// İSTER 2: SOAP Endpoint Tanımlama (20 Puan)
// Dışarıdan "http://localhost:port/Service.asmx" adresinden SOAP isteği alabilmeyi sağlar.
app.UseSoapEndpoint<ILogService>("/Service.asmx", new SoapEncoderOptions());

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();