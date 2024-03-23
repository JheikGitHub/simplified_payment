using FluentValidation;
using PaymentPicPay.API.Models;

namespace PaymentPicPay.API.Validators
{
    public class TransactionValidator : AbstractValidator<Transaction>
    {
        public TransactionValidator()
        {
            RuleFor(x => x.SendId).NotNull();
            RuleFor(x => x.ReceiveId).NotNull();
            RuleFor(x => x.Amount).NotNull().GreaterThan(0);
            RuleFor(x => x.TransactionType).IsInEnum();
        }
    }
}
