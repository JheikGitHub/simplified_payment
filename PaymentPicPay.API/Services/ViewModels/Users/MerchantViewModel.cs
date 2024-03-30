using PaymentPicPay.API.Domain.Models;

namespace PaymentPicPay.API.Services.ViewModels.Users
{
    public class MerchantViewModel : UserViewModel
    {
        public MerchantViewModel() { }
        public MerchantViewModel(Merchant merchant)
        {
            FullName = merchant.FullName;
            CNPJ = merchant.CNPJ;
            Password = merchant.Password;
            Email = merchant.Email.Address;
            Wallet = merchant.Wallet.Balance;
        }
        public string CNPJ { get; set; }
    }
}
