using PaymentPicPay.API.Data.Repositories.Shared;
using MerchantEntity = PaymentPicPay.API.Domain.Models.Merchant;

namespace PaymentPicPay.API.Data.Repositories.Merchant
{
    public interface IMerchantRepository : 
        IRepositoryBase<MerchantEntity>
    {

    }
}
