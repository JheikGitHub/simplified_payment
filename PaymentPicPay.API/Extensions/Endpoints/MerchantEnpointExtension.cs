using PaymentPicPay.API.Data.Repositories._RepositoryWrapper;
using PaymentPicPay.API.Domain.Models;
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
                (IRepositoryWrapper repository) =>
                {
                    try
                    {
                        return Results.Ok(await repository.MerchantRepository.GetAllAsync());
                    }
                    catch (Exception)
                    {
                        return Results.StatusCode(HttpStatusCode.InternalServerError.GetHashCode());
                    }
                })
                .WithName("GetAllMerchants")
                .WithOpenApi(options =>
                {
                    options.Description = "Get all Merchants.";
                    options.Summary = "Get all Merchants.";
                    return options;
                })
                .Produces<IEnumerable<Customer>>(statusCode: 200)
                .Produces(statusCode: 500); ;
            #endregion

            return app;
        }
    }
}
