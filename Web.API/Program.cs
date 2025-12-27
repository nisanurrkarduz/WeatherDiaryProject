using SoapCore;
using Web.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSoapCore();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddControllers(); 

var app = builder.Build();

app.UseRouting();

// SOAP servisini /Service.asmx adresinden yayına al (20 Puanlık SOAP İsteri)
app.UseSoapEndpoint<ILogService>("/Service.asmx", new SoapEncoderOptions());

app.MapControllers(); 

app.Run("http://localhost:5001");