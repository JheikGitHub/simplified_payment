
namespace PaymentPicPay.API.Repositories
{
    public interface IRedisRepository
    {
        Task SetAsync(string key, string value);
        Task<string> GetStringAsync(string key);
        Task<byte[]> GetAsync(string key);
    }
}
