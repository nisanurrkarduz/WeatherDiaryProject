using Grpc.Core;

namespace Web.GrpcService.Services;

public class AdviceServiceImpl : AdviceService.AdviceServiceBase
{
    public override Task<AdviceReply> GetAdvice(AdviceRequest request, ServerCallContext context)
    {
        var mood = (request.Mood ?? "").Trim();
        var t = request.Temperature;
        var p = request.Precipitation;

        string advice;

        if (p > 0)
            advice = "Yağmur var 🌧️ Şemsiye al. Trafikteysen sakin müzik iyi gelebilir 🎧";
        else if (mood.Equals("Stresli", StringComparison.OrdinalIgnoreCase))
            advice = "Stresliysen trafikte sakin bir playlist aç, kısa nefes egzersizi dene 🎧";
        else if (mood.Equals("Mutlu", StringComparison.OrdinalIgnoreCase) && t >= 22)
            advice = "Hava güzel 🌞 Kısa bir yürüyüş veya açık havada kahve iyi gider.";
        else if (t < 10)
            advice = "Hava soğuk ❄️ Kalın giyin, sıcak bir içecek iyi gelir.";
        else
            advice = "Gününü planlarken hava durumunu takip etmeyi unutma 🙂";

        return Task.FromResult(new AdviceReply { Advice = advice });
    }
}
