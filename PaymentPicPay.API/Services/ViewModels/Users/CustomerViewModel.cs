using PaymentPicPay.API.Domain.Models;

namespace PaymentPicPay.API.Services.ViewModels.Users
{
    public class CustomerViewModel : UserViewModel
    {
        public CustomerViewModel() { }

        public CustomerViewModel(Customer customer)
        {
            FullName = customer.FullName;
            CPF = customer.CPF;
            Password = customer.Password;
            Email = customer.Email.Address;
            Wallet = customer.Wallet.Balance;
        }

        public string CPF { get; set; }
    }
}
