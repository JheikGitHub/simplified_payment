using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PaymentPicPay.API.Data.Caching;
using PaymentPicPay.API.Data.Context;
using PaymentPicPay.API.Domain.Models;
using PaymentPicPay.API.Domain.Validators;

namespace PaymentPicPay.API.Extensions
{
    public static class BuilderExtension
    {
        public static void AddValidators(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IValidator<Transaction>, TransactionValidator>();
        }
    }
}
