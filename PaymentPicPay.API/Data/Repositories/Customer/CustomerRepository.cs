using PaymentPicPay.API.Data.Context;
using PaymentPicPay.API.Data.Repositories.Shared;
using CustomerEntity = PaymentPicPay.API.Domain.Models.Customer;

namespace PaymentPicPay.API.Data.Repositories.Customer
{
    public class CustomerRepository : 
        RepositoryBase<CustomerEntity>, 
        ICustomerRepository
    {
        public CustomerRepository(PaymentDataContext context)
            : base(context) { }
    }
}
