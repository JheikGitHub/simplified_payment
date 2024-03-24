using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PaymentPicPay.API.Data.Caching;
using PaymentPicPay.API.Data.Context;
using PaymentPicPay.API.Domain.Models;
using PaymentPicPay.API.Domain.Validators;
using PaymentPicPay.API.Extensions;
using PaymentPicPay.API.Extensions.Endpoints;
using PaymentPicPay.API.Services.Externals.EmailSendService;
using PaymentPicPay.API.Services.Externals.TransferAuthorizer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

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

//Inicializar DbInitilizer
app.UseDbInitializer();

//Endpoints
app.UseCustomerEndpoints()
    .UseMerchantEndpoints()
    .UseTransactionEnpoints();

app.Run();

