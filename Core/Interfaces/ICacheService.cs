using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedCacheSample.Core.Interfaces
{
    public interface ICacheService
    {
        Task SetStringAsync(string key, string value);
        Task<string> GetStringAsync(string key);
        Task<bool> KeyExistsAsync(string key);
        Task<string> GetOrAddByLockAsync(string key, string value, TimeSpan expiration, TimeSpan lockExpiration);
    }
}
