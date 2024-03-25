using Microsoft.EntityFrameworkCore;
using PaymentPicPay.API.Data.Caching;
using PaymentPicPay.API.Data.Context;
using PaymentPicPay.API.Data.Repositories._RepositoryWrapper;
using PaymentPicPay.API.Data.Repositories.Customer;
using PaymentPicPay.API.Data.Repositories.Merchant;
using PaymentPicPay.API.Data.Repositories.Shared;
using PaymentPicPay.API.Data.Repositories.Transaction;
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

            //Repositories
            //builder.Services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            //builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            //builder.Services.AddScoped<IMerchantRepository, MerchantRepository>();
            //builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

            builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

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
