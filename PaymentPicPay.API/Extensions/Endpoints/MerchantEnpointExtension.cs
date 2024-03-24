using Microsoft.EntityFrameworkCore;
using PaymentPicPay.API.Data.Context;
using System.Net;

namespace PaymentPicPay.API.Extensions.Endpoints
{
    public static class MerchantEnpointExtension
    {
        public static WebApplication UseMerchantEndpoints(this WebApplication app)
        {
            #region Merchant
            app.MapGet(
                "v1/api/merchants",
                async
                (PaymentDataContext context) =>
                {
                    try
                    {
                        return Results.Ok(await context.Merchants.ToListAsync());
                    }
                    catch (Exception)
                    {
                        return Results.StatusCode(HttpStatusCode.InternalServerError.GetHashCode());
                    }
                }).WithName("GetAllMerchants");
            #endregion

            return app;
        }
    }
}
