using PaymentPicPay.API.Extensions;
using PaymentPicPay.API.Extensions.Endpoints;
using PaymentPicPay.API.Extensions.IoC;

var builder = WebApplication.CreateBuilder(args);

// Add HttpClientFactory
builder.Services.AddHttpClient();

builder.Services.AddEndpointsApiExplorer();
builder.AddSwagger();

builder.RegisterServices();

builder.AddValidators();

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

