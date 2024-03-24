using Microsoft.EntityFrameworkCore;
using PaymentPicPay.API.Data.Caching;
using PaymentPicPay.API.Data.Context;
using PaymentPicPay.API.Services.Externals.EmailSendService;
using PaymentPicPay.API.Services.Externals.TransferAuthorizer;

namespace PaymentPicPay.API.Extensions.IoC
{
    public static class NativeInjectorConfig
    {
        public static void RegisterServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<PaymentDataContext>(
                options => options.UseInMemoryDatabase("PaymentPicPay"));

            builder.Services.AddScoped<DbInitializer>();

            //External Services
            builder.Services.AddTransient<IEmailSendService, EmailSendService>();
            builder.Services.AddTransient<ITransferAuthorizerService, TransferAuthorizerService>();


            //Caching redis
            builder.Services.AddScoped<IRedisRepository, RedisRepository>();
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost:6379";
            });
        }
    }
}
