
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;

namespace MetadataService.SharedKernel
{
    public static class DistributedCaching
    {
        public async static Task SetAsync<T>(this IDistributedCache distributedCache, string key, T value,  CancellationToken token = default(CancellationToken))
        {
            var jsonData = JsonConvert.SerializeObject(value);
            await distributedCache.SetStringAsync(key, jsonData, new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24) }, token);
        }

        public async static Task<T> GetAsync<T>(this IDistributedCache distributedCache, string key, CancellationToken token = default(CancellationToken)) where T : class
        {

            var result = await distributedCache.GetStringAsync(key, token);
            if (result != null)
                return JsonConvert.DeserializeObject<T>(result);
            else
                return null;
        }
        public async static Task Remove(this IDistributedCache distributedCache, string key,CancellationToken token = default(CancellationToken))
        {
            await distributedCache.RemoveAsync(key, token);
        }
    }
}
