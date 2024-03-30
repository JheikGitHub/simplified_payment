using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PaymentPicPay.API.Domain.Models;

namespace PaymentPicPay.API.Data.Mappins
{
    public class MerchantMap : IEntityTypeConfiguration<Merchant>
    {
        public void Configure(EntityTypeBuilder<Merchant> builder)
        {
            builder.ToTable("Merchant");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Password)
                .HasColumnName("Password")
                .HasColumnType("varchar(200)")
                .IsRequired();

            builder.Property(x => x.CNPJ)
                .HasColumnName("Cpf")
                .HasColumnType("varchar(14)")
                .IsRequired();

            builder.Property(x => x.CreatedDate)
                .HasColumnName("CreatedAt")
                .HasDefaultValue(DateTime.UtcNow)
                .IsRequired();

            builder.Property(x => x.UpdatedDate)
                .HasColumnName("UpdatedAt");

            builder.OwnsOne(x => x.Email)
                .Property(email => email.Address)
                .HasColumnName("Address")
                .HasColumnType("varchar(200)")
                .IsRequired();

            builder.OwnsOne(x => x.Wallet)
                .Property(email => email.Balance)
                .HasColumnName("Balance")
                .IsRequired();

            //CNPJ e Email deve ser unico
            builder.HasIndex(x => x.CNPJ).IsUnique();
            builder.OwnsOne(x => x.Email).HasIndex(index => index.Address).IsUnique();

            builder.HasMany(x => x.Transactions);
        }
    }
}
