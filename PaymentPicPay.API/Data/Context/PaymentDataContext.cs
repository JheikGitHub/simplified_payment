using Microsoft.EntityFrameworkCore;
using PaymentPicPay.API.Data.Mappins;
using PaymentPicPay.API.Domain.Models;

namespace PaymentPicPay.API.Data.Context
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
