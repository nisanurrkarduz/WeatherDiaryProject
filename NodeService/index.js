const express = require("express");
const axios = require("axios");

const app = express();
const port = 3000;


app.get("/tahmin", async (req, res) => {
  try {
    const lat = parseFloat(req.query.lat) || 41.0082; 
    const lng = parseFloat(req.query.lng) || 28.9784; 

    const response = await axios.get("https://api.open-meteo.com/v1/forecast", {
      params: {
        latitude: lat,
        longitude: lng,
        current: "temperature_2m,precipitation",
        timezone: "auto",
      },
    });

    const sicaklik = response.data.current.temperature_2m;
    const yagis = response.data.current.precipitation;

    let durum = "Nötr";
    if (yagis > 0) durum = "Stresli";
    else if (sicaklik >= 24) durum = "Mutlu";
    else if (sicaklik < 10) durum = "Stresli";

    res.json({
      durum,
      sicaklik,
      yagis,
      mesaj: `Hava ${sicaklik}°C, yağış: ${yagis} mm. Node.js analizi tamamlandı.`,
      lat,
      lng,
      kaynak: "Open-Meteo API",
    });
  } catch (error) {
    res
      .status(500)
      .json({ hata: "Dış API bağlantı hatası.", detay: error.message });
  }
});

app.listen(port, () =>
  console.log(`✅ Node.js SOA API ${port} portunda aktif`)
);
