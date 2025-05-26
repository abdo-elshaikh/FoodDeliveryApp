using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace FoodDeliveryApp.Extensions
{
    public static class DistributedCacheExtensions
    {
        public static async Task<string> GetAsync(this IDistributedCache cache, string key)
        {
            var value = await cache.GetStringAsync(key);
            return value ?? string.Empty;
        }

        public static async Task SetAsync<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions? options = null)
        {
            if (value == null)
            {
                await cache.RemoveAsync(key);
                return;
            }
            var json = JsonSerializer.Serialize(value);
            if (json == null)
            {
                await cache.RemoveAsync(key);
                return;
            }
            await cache.SetStringAsync(key, json, options ?? new DistributedCacheEntryOptions());
        }
    }
}