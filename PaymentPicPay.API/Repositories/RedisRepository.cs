using Microsoft.Extensions.Caching.Distributed;

namespace PaymentPicPay.API.Repositories
{
    public class RedisRepository : IRedisRepository
    {
        private readonly IDistributedCache _cache;
        public RedisRepository(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<byte[]> GetAsync(string key)
        {
            return await _cache.GetAsync(key);
        }

        public async Task<string> GetStringAsync(string key)
        {
            return await _cache.GetStringAsync(key);
        }

        public async Task SetAsync(string key, string value)
        {
            await _cache.SetStringAsync(key, value);
        }
    }
}
