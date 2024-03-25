using PaymentPicPay.API.Data.Context;
using PaymentPicPay.API.Data.Repositories.Shared;
using MerchantEntity = PaymentPicPay.API.Domain.Models.Merchant;

namespace PaymentPicPay.API.Data.Repositories.Merchant
{
    public class MerchantRepository : 
        RepositoryBase<MerchantEntity>, 
        IMerchantRepository
    {
        public MerchantRepository(PaymentDataContext context)
            : base(context) { }
    }
}
