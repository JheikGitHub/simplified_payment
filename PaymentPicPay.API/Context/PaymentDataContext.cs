using Microsoft.EntityFrameworkCore;
using PaymentPicPay.API.Mappins;
using PaymentPicPay.API.Models;

namespace PaymentPicPay.API.Context
{
    public class PaymentDataContext : DbContext
    {
        public PaymentDataContext(DbContextOptions options)
            : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfiguration(new TransactionMap());
            modelBuilder.ApplyConfiguration(new MerchantMap());
            modelBuilder.ApplyConfiguration(new CustomerMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}
