using PaymentPicPay.API.Data.Repositories.Shared;
using TransactionEntity = PaymentPicPay.API.Domain.Models.Transaction;

namespace PaymentPicPay.API.Data.Repositories.Transaction
{
    public interface ITransactionRepository :
        IRepositoryBase<TransactionEntity>
    {
        Task<IEnumerable<TransactionEntity>> GetAllIncludes(); 
    }
}
