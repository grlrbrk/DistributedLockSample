using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistributedCacheSample.Core.Interfaces;

namespace DistributedCacheSample.Application.Services
{
    public class CacheService : ICacheService
    {
        private readonly ICacheService _cacheService;

        public CacheService(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public Task SetStringAsync(string key, string value)
        {
            return _cacheService.SetStringAsync(key, value);
        }

        public async Task<string> GetStringAsync(string key)
        {
            return await _cacheService.GetStringAsync(key);
        }

        public async Task<bool> KeyExistsAsync(string key)
        {
            return await _cacheService.KeyExistsAsync(key);
        }
        public async Task<string> GetOrAddByLockAsync(string key, string value, TimeSpan expiration, TimeSpan lockExpiration)
        {
            return await _cacheService.GetOrAddByLockAsync(key, value, expiration, lockExpiration);
        }
    }
}
