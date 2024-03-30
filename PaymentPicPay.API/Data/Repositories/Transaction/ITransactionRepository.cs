using PaymentPicPay.API.Data.Repositories.Shared;
using PaymentPicPay.API.Services.ViewModels.Transactions;
using TransactionEntity = PaymentPicPay.API.Domain.Models.Transaction;

namespace PaymentPicPay.API.Data.Repositories.Transaction
{
    public interface ITransactionRepository :
        IRepositoryBase<TransactionEntity>
    {
        Task<IEnumerable<TransactionViewModel>> GetAllIncludes(); 
    }
}
