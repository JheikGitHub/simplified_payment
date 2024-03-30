using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentPicPay.API.Domain.Models;

namespace PaymentPicPay.API.Data.Mappins
{
    public class TransactionB2CMap : IEntityTypeConfiguration<TransactionB2C>
    {
        public void Configure(EntityTypeBuilder<TransactionB2C> builder)
        {
            builder.ToTable("Transfers");

            builder.Property(x => x.SendId)
                .HasColumnName("SendId")
                .IsRequired();

            builder.Property(x => x.ReceiveId)
                .HasColumnName("ReceivedId")
                .IsRequired();

            builder.Property(x => x.Amount)
                .HasColumnName("Amount")
                //.HasColumnType("decimal(5,2)")
                .IsRequired();

            builder.Property(x => x.OperationStatus)
                .HasColumnName("EOperationStatus")
                //.HasColumnType("decimal(5,2)")
                .IsRequired();

            builder.Property(x => x.CreatedDate)
                .HasColumnName("CreatedAt")
                .HasDefaultValue(DateTime.UtcNow)
                .IsRequired();

            builder.Property(x => x.UpdatedDate)
                .HasColumnName("UpdatedAt");

            builder.HasOne(x=>x.Receive)
                .WithMany(x=>x.Transactions)
                .HasForeignKey(x=>x.ReceiveId);
        }
    }
}
