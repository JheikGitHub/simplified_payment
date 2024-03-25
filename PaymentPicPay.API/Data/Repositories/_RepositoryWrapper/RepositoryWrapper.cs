using PaymentPicPay.API.Data.Context;
using PaymentPicPay.API.Data.Repositories.Customer;
using PaymentPicPay.API.Data.Repositories.Merchant;
using PaymentPicPay.API.Data.Repositories.Transaction;

namespace PaymentPicPay.API.Data.Repositories._RepositoryWrapper
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly PaymentDataContext _context;
        private ICustomerRepository _customerRepository;
        private IMerchantRepository _merchantRepository;
        private ITransactionRepository _transactionRepository;
        public RepositoryWrapper(PaymentDataContext context)
        {
            _context = context;
        }
        public ICustomerRepository CustomerRepository
        {
            get
            {
                _customerRepository ??= new CustomerRepository(_context);

                return _customerRepository;
            }
        }

        public IMerchantRepository MerchantRepository
        {
            get
            {
                _merchantRepository ??= new MerchantRepository(_context);
                return _merchantRepository;
            }
        }

        public ITransactionRepository TransactionRepository
        {
            get
            {
                _transactionRepository ??= new TransactionRepository(_context);
                return _transactionRepository;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
