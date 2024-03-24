using PaymentPicPay.API.Domain.Models;
using PaymentPicPay.API.Domain.ValueObjects;

namespace PaymentPicPay.API.Data.Context
{
    public class DbInitializer : IDisposable
    {
        private readonly PaymentDataContext _context;
        public DbInitializer(PaymentDataContext context)
        {
            _context = context;
        }

        public void Run()
        {
            #region Customer
            var DbSetCustomer = _context.Set<Customer>();
            if (!DbSetCustomer.Any())
            {
                ICollection<Customer> customers = [
                    new Customer("Jheik Alves", new Email("Jheikalves7@gmail.com"), "Jheik@123", new Wallet(1000), "14394819232"),
                    new Customer("Jose da Silva", new Email("silva@gmail.com"), "Silva@123", new Wallet(1000), "12345678910"),
                    new Customer("Antonio Ribeiro", new Email("Antonio@gmail.com"), "Antonio@123", new Wallet(1000), "12345678910"),
                    ];

                DbSetCustomer.AddRange(customers);
            }
            #endregion

            #region Merchant
            var DbSetMerchant = _context.Set<Merchant>();
            if (!DbSetMerchant.Any())
            {
                ICollection<Merchant> merchants = [
                    new Merchant("Banco Itaú",
                                 new Email("itau@gmail.com"),
                                 "@itau123",
                                 new Wallet(100000),
                                 "39.930.585/0001-84"),

                    new Merchant("Magazine Luiza",
                                 new Email("magazine_luiza@gmail.com"),
                                 "@123",
                                 new Wallet(100000),
                                 "07.241.916/0001-82"),
                    ];

                DbSetMerchant.AddRange(merchants);
            }
            //Salvar para poder fazer a transferencia abaixo
            _context.SaveChanges();
            #endregion
        }
        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
