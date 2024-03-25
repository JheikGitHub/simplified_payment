using PaymentPicPay.API.Data.Repositories.Shared;
using System.Linq.Expressions;
using CustomerEntity = PaymentPicPay.API.Domain.Models.Customer;

namespace PaymentPicPay.API.Data.Repositories.Customer
{
    public interface ICustomerRepository : IRepositoryBase<CustomerEntity>
    {
    }
}
