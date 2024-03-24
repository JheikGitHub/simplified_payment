using PaymentPicPay.API.Data.Caching;
using PaymentPicPay.API.Data.Context;
using PaymentPicPay.API.Domain.Models;
using System.Text;
using System.Text.Json;

namespace PaymentPicPay.API.Extensions.Endpoints
{
    public static class CustomerEnpointExtension
    {
        public static WebApplication UseCustomerEndpoints(this WebApplication app)
        {
            #region Customer
            app.MapGet(
                "/v1/api/customers",
                async (
                    PaymentDataContext context,
                    IRedisRepository redis) =>
                {
                    try
                    {
                        IEnumerable<Customer> customers;

                        //caching
                        var cacheKey = "orderList";
                        var cacheValue = await redis.GetAsync(cacheKey);

                        if (cacheValue != null)
                            customers = JsonSerializer.Deserialize<IEnumerable<Customer>>(Encoding.UTF8.GetString(cacheValue));
                        else
                        {
                            customers = [.. context.Customers];
                            await redis.SetAsync(cacheKey, JsonSerializer.Serialize(customers));
                        }

                        return Results.Ok(customers);
                    }
                    catch (Exception e)
                    {
                        return Results.Problem(
                            e.Message,
                            statusCode: StatusCodes.Status500InternalServerError,
                            title: "Error in get all customers");
                    }
                }).WithName("GetAllCustomers")
                .WithOpenApi(options => { options.Description = "Get all customers."; options.Summary = "This is a summary"; return options; })
                .Produces<IEnumerable<Customer>>(statusCode: 200)
                .Produces(statusCode: 500);

            #endregion

            return app;
        }
    }
}
