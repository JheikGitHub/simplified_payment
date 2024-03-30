using PaymentPicPay.API.Domain.Enums;
using PaymentPicPay.API.Services.ViewModels.Users;

namespace PaymentPicPay.API.Services.ViewModels.Transactions
{
    public class TransactionViewModel
    {
        public TransactionViewModel() { }

        public UserViewModel Send { get; set; }
        public UserViewModel Receive { get; set; }
        public decimal Amount { get; set; }
        public EOperationStatus OperationStatus { get; set; }
        public ETransactionType TransactionType { get; set; }
    }
}
