using PaymentPicPay.API.Data.Context;

namespace PaymentPicPay.API.Extensions
{
    public static class AppExtension
    {
        public static WebApplication UseDbInitializer(this WebApplication app)
        {
            var scope = app.Services.CreateScope();
            var provider = scope.ServiceProvider;
            var DbInitializer = provider.GetRequiredService<DbInitializer>();
            DbInitializer.Run();

            return app;
        }
    }
}
