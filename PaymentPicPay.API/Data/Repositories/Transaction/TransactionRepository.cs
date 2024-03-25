using Microsoft.EntityFrameworkCore;
using PaymentPicPay.API.Data.Context;
using PaymentPicPay.API.Data.Repositories.Shared;
using TransactionEntity = PaymentPicPay.API.Domain.Models.Transaction;

namespace PaymentPicPay.API.Data.Repositories.Transaction
{
    public class TransactionRepository : 
        RepositoryBase<TransactionEntity>, 
        ITransactionRepository
    {
        private readonly PaymentDataContext _context;
        public TransactionRepository(PaymentDataContext context) 
            : base(context) { }

        public async Task<IEnumerable<TransactionEntity>> GetAllIncludes()
        {
            return await _context.Transactions
                .Include(send => send.SendUser)
                .Include(receiver => receiver.ReceiverUser)
                .ToListAsync();
        }
    }
}
