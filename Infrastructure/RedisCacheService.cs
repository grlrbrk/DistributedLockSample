using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistributedCacheSample.Core.Interfaces;
using StackExchange.Redis;

namespace DistributedCacheSample.Infrastructure
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase _database;

        public RedisCacheService(string connectionString)
        {
            var connection = ConnectionMultiplexer.Connect(connectionString);
            _database = connection.GetDatabase();
        }

        public async Task SetStringAsync(string key, string value)
        {
            await _database.StringSetAsync(key, value);
        }

        public async Task<string> GetStringAsync(string key)
        {
            return await _database.StringGetAsync(key);
        }

        public async Task<bool> KeyExistsAsync(string key)
        {
            return await _database.KeyExistsAsync(key);
        }
        public async Task<string> GetOrAddByLockAsync(string key, string value, TimeSpan expiration, TimeSpan lockExpiration)
        {
            var val = await _database.StringGetAsync(key);

            if (!val.HasValue) // Eğer değer yoksa
            {
                var lockKey = key + "Lock";
                string lockValue = Environment.MachineName;
                
                bool locked = await _database.LockTakeAsync(lockKey, lockValue, lockExpiration);
                try
                {
                    while (!locked && !val.HasValue)
                    {
                        await Task.Delay(500);
                        locked = await _database.LockTakeAsync(lockKey, lockValue, lockExpiration);
                        val = await _database.StringGetAsync(key);
                    }

                    // Eğer hala değer yoksa yeni değeri ekleyelim
                    if (locked && !val.HasValue)
                    {
                        await _database.StringSetAsync(key, value, expiration);
                        val = value;
                    }
                }
                finally
                {
                    // Kilidi bırak
                    if (locked)
                    {
                        await _database.LockReleaseAsync(lockKey, lockValue);
                    }
                }
            }

            return val.ToString();
        }
    }
}
