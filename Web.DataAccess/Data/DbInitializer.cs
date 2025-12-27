using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Web.Entity; 

namespace Web.DataAccess 
{
    public static class DbInitializer
    {
        public static void Initialize(AllprojectContext context)
        {
            
           
            //context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return;
            }

            // 1. ROLLER
            var roles = new Role[]
            {
                new Role { RoleId = 1, RoleName = "Admin" },
                new Role { RoleId = 2, RoleName = "User" }
            };
            foreach (var r in roles) { context.Roles.Add(r); }
            context.SaveChanges();

            // 2. HAVA TİPLERİ
            var weatherTypes = new WeatherType[]
            {
                new WeatherType { TypeId = 1, TypeName = "Güneşli", RiskLevel = 1 },
                new WeatherType { TypeId = 2, TypeName = "Bulutlu", RiskLevel = 2 },
                new WeatherType { TypeId = 3, TypeName = "Yağmurlu", RiskLevel = 3 },
                new WeatherType { TypeId = 4, TypeName = "Fırtınalı", RiskLevel = 5 },
                new WeatherType { TypeId = 5, TypeName = "Karlı", RiskLevel = 4 }
            };
            foreach (var w in weatherTypes) { context.WeatherTypes.Add(w); }
            context.SaveChanges();

            // 3. ŞEHİRLER
            var cities = new City[]
            {
                new City { CityId = 1, CityName = "İstanbul", Country = "Türkiye" },
                new City { CityId = 2, CityName = "Ankara", Country = "Türkiye" },
                new City { CityId = 3, CityName = "İzmir", Country = "Türkiye" },
                new City { CityId = 4, CityName = "Antalya", Country = "Türkiye" },
                new City { CityId = 5, CityName = "Bursa", Country = "Türkiye" }
            };
            foreach (var c in cities) { context.Cities.Add(c); }
            context.SaveChanges();

            // 4. KULLANICILAR
            var users = new User[]
            {
                new User {
                    Username = "admin",
                    Email = "admin@ogece.com",
                    PasswordHash = "123",
                    RoleId = 1,
                    CreatedAt = DateTime.Now
                },
                new User {
                    Username = "user",
                    Email = "user@ogece.com",
                    PasswordHash = "123",
                    RoleId = 2,
                    CreatedAt = DateTime.Now
                }
            };
            foreach (var u in users) { context.Users.Add(u); }
            context.SaveChanges();

            // 5. ÖRNEK LOGLAR
            var logs = new DailyLog[]
            {
                new DailyLog { CityId = 1, UserId = 1, LogDate = DateTime.Now.AddDays(-1), Temperature = 26, Humidity = 45, WeatherTypeId = 1 },
                new DailyLog { CityId = 2, UserId = 2, LogDate = DateTime.Now, Temperature = 15, Humidity = 60, WeatherTypeId = 2 }
            };
            foreach (var l in logs) { context.DailyLogs.Add(l); }
            context.SaveChanges();

            // SQL Komutları
            context.Database.ExecuteSqlRaw(@"
                CREATE OR REPLACE VIEW ""View_UserLogDetails"" AS
                SELECT u.username as ""Username"", 
                       c.city_name as ""CityName"", 
                       d.temperature as ""Temperature"", 
                       d.log_date as ""LogDate"", 
                       w.type_name as ""WeatherType""
                FROM ""DailyLogs"" d
                JOIN ""Users"" u ON d.user_id = u.user_id
                JOIN ""Cities"" c ON d.city_id = c.city_id
                JOIN ""WeatherTypes"" w ON d.weather_type_id = w.type_id;
            ");

            context.Database.ExecuteSqlRaw(@"
                CREATE OR REPLACE FUNCTION sp_GetHighTempLogs(temp_val decimal)
                RETURNS TABLE ( ""LogId"" integer, ""CityName"" text, ""Temperature"" decimal ) 
                LANGUAGE plpgsql AS $$
                BEGIN
                    RETURN QUERY
                    SELECT d.log_id, c.city_name::text, d.temperature
                    FROM ""DailyLogs"" d
                    JOIN ""Cities"" c ON d.city_id = c.city_id
                    WHERE d.temperature > temp_val;
                END; $$;
            ");

            context.Database.ExecuteSqlRaw(@"
                CREATE OR REPLACE FUNCTION fn_GetCityLogCount(cityId int)
                RETURNS integer LANGUAGE plpgsql AS $$
                DECLARE log_count integer;
                BEGIN
                    SELECT count(*) INTO log_count FROM ""DailyLogs"" WHERE city_id = cityId;
                    RETURN log_count;
                END; $$;
            ");
        }
    }
}