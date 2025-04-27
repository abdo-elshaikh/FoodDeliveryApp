using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace FoodDeliveryApp.Extensions
{
    public static class DistributedCacheExtensions
    {
        public static async Task<T> GetAsync<T>(this IDistributedCache cache, string key)
        {
            var value = await cache.GetStringAsync(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }

        public static async Task SetAsync<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options = null)
        {
            var json = JsonSerializer.Serialize(value);
            await cache.SetStringAsync(key, json, options ?? new DistributedCacheEntryOptions());
        }
    }
}