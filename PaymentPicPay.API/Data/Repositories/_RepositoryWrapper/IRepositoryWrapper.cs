using PaymentPicPay.API.Data.Repositories.Customer;
using PaymentPicPay.API.Data.Repositories.Merchant;
using PaymentPicPay.API.Data.Repositories.Transaction;

namespace PaymentPicPay.API.Data.Repositories._RepositoryWrapper
{
    public interface IRepositoryWrapper : IDisposable
    {
        ICustomerRepository CustomerRepository { get; }
        IMerchantRepository MerchantRepository { get; }
        ITransactionRepository TransactionRepository { get; }
        Task SaveChangesAsync();
    }
}
