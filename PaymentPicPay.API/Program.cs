using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PaymentPicPay.API.Data.Caching;
using PaymentPicPay.API.Data.Context;
using PaymentPicPay.API.Domain.Enums;
using PaymentPicPay.API.Domain.Models;
using PaymentPicPay.API.Domain.Validators;
using PaymentPicPay.API.Services.Externals.EmailSendService;
using PaymentPicPay.API.Services.Externals.TransferAuthorizer;
using PaymentPicPay.API.Services.ViewModels;
using System.Net;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PaymentDataContext>(
    options => options.UseInMemoryDatabase("PaymentPicPay"));

builder.Services.AddScoped<DbInitializer>();

//Validators
builder.Services.AddScoped<IValidator<Transaction>, TransactionValidator>();

//Caching redis
builder.Services.AddScoped<IRedisRepository, RedisRepository>();

//External Services
builder.Services.AddTransient<IEmailSendService, EmailSendService>();
builder.Services.AddTransient<ITransferAuthorizerService, TransferAuthorizerService>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// inicializar DbInitilizer
var scope = app.Services.CreateScope();
var provider = scope.ServiceProvider;
var DbInitializer = provider.GetRequiredService<DbInitializer>();
DbInitializer.Run();

//Endpoints

#region Customer
app.MapGet(
    "/v1/api/customers",
    async (
        PaymentDataContext context,
        IRedisRepository redis) =>
    {
        try
        {
            var cacheKey = "orderList";
            IEnumerable<Customer> customers;

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

#region Transaction
app.MapPost("v1/api/transaction",
    async
    (TransactionViewModel viewModel,
    IValidator<Transaction> Validator,
    ITransferAuthorizerService authorizationService,
    IEmailSendService emailSendService,
    PaymentDataContext context) =>
    {
        try
        {
            if (viewModel == null)
                return Results.BadRequest("The transaction data is invalid.");

            #region Buscar usuarios

            User userSend = context.Customers.FirstOrDefault(send => send.Id == viewModel.SendId);

            User userReceived = null;
            switch (viewModel.TransactionType)
            {
                case ETransactionType.B2B:
                    userReceived = context.Customers.FirstOrDefault(received => received.Id == viewModel.ReceiveId);
                    break;
                case ETransactionType.B2C:
                    userReceived = context.Merchants.FirstOrDefault(received => received.Id == viewModel.ReceiveId);
                    break;

            }

            #endregion

            #region Criar transação
            Transaction transaction = new(
                userSend.Id,
                userReceived.Id,
                viewModel.Amount,
                viewModel.TransactionType);

            #endregion

            #region Validar os dados da transação
            var result = Validator.Validate(transaction);

            if (!result.IsValid)
                return Results.ValidationProblem(result.ToDictionary());

            var authorizationTransfer = await authorizationService.AuthorizationTranfer();

            if (!authorizationTransfer)
                return Results.Problem(
                    detail: "Error when making the transaction.",
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "External authorizer");

            #endregion

            #region Realizar a transação

            var resultTransaction = transaction.Transfer(userSend, userReceived);

            #endregion

            #region Notificação de recebimento
            if (resultTransaction)
                await emailSendService.SendNotificationTransaction(userReceived);

            #endregion

            await context.SaveChangesAsync();

            return Results.Ok();
        }
        catch (Exception e)
        {
            return Results.Problem(
                detail: e.Message,
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Error when making the transaction.");
        }
    });
#endregion


app.Run();

