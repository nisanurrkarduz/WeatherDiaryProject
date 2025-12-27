    using Web.GrpcService.Services;

    var builder = WebApplication.CreateBuilder(args);

    // gRPC servislerini ekle
    builder.Services.AddGrpc();

    var app = builder.Build();

    // gRPC endpointleri
        
    app.MapGrpcService<AdviceServiceImpl>();   // Bizim tavsiye servisi (asıl)

    // Basit kontrol endpointi
    app.MapGet("/", () => "✅ gRPC server çalışıyor. gRPC çağrıları client ile yapılır.");

    app.Run("http://localhost:7001");
