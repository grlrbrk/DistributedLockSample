using DistributedCacheSample.Application.Services;
using DistributedCacheSample.Core.Interfaces;
using DistributedCacheSample.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DistributedCacheSample
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    string redisConnectionString = "localhost:6379";

                    services.AddSingleton<ICacheService>(new RedisCacheService(redisConnectionString));
                    services.AddSingleton<CacheService>(); 
                }).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var cacheService = services.GetRequiredService<CacheService>();

                // 📌 Cache için anahtar ve değer belirleyelim
                string cacheKey = "token";
                string cacheValue = "61231267asasd1612312";
                TimeSpan expiration = TimeSpan.FromMinutes(5);
                TimeSpan lockExpiration = TimeSpan.FromSeconds(10);

                // 🔒 Kilitleyerek cache'e veri ekle (await ile çağır!)
                string result = await cacheService.GetOrAddByLockAsync(cacheKey, cacheValue, expiration, lockExpiration);

                // 🖥️ Sonucu ekrana yazdır
                Console.WriteLine($"Cache'ten gelen değer: {result}");
            }
        }
    }
}
