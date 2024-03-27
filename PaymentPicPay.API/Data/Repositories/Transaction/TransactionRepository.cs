using Microsoft.EntityFrameworkCore;
using PaymentPicPay.API.Data.Context;
using PaymentPicPay.API.Data.Repositories.Shared;
using PaymentPicPay.API.Domain.Enums;
using PaymentPicPay.API.Services.ViewModels;
using TransactionEntity = PaymentPicPay.API.Domain.Models.Transaction;

namespace PaymentPicPay.API.Data.Repositories.Transaction
{
    public class TransactionRepository :
        RepositoryBase<TransactionEntity>,
        ITransactionRepository
    {
        private readonly PaymentDataContext _context;
        public TransactionRepository(PaymentDataContext context)
            : base(context) 
        { 
            _context = context; 
        }

        public async Task<IEnumerable<TransactionViewModel>> GetAllIncludes()
        {
            var transactions = _context.Transactions.AsQueryable().AsNoTracking();
            var customers = _context.Customers.AsQueryable().AsNoTracking();
            var merchants = _context.Merchants.AsQueryable().AsNoTracking();

            var query = from transaction in transactions
                        join customer in customers on transaction.SendId equals customer.Id
                        select new TransactionViewModel
                        {
                            Amount = transaction.Amount,
                            SendId = transaction.SendId,
                            TransactionType = transaction.TransactionType,
                            Send = new CustomerViewModel
                            {
                                CPF = customer.CPF,
                                Email = customer.Email.Address,
                                FullName = customer.FullName,
                                //Password = customer.Password,
                                Wallet = customer.Wallet.Balance
                            },
                            Receive = transaction.TransactionType == ETransactionType.B2B ?
                            new CustomerViewModel(customers.FirstOrDefault(cus => cus.Id == transaction.ReceiveId)) :
                            new MerchantViewModel(merchants.FirstOrDefault(mer => mer.Id == transaction.ReceiveId))
                        };

            return await query.ToListAsync();
        }

    }
}
