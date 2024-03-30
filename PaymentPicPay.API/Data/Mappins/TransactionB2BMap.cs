using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentPicPay.API.Domain.Models;

namespace PaymentPicPay.API.Data.Mappins
{
    public class TransactionB2BMap : IEntityTypeConfiguration<TransactionB2B>
    {
        public void Configure(EntityTypeBuilder<TransactionB2B> builder)
        {
            builder.ToTable("TransfersB2B");

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

            builder.HasOne(x => x.Send)
                .WithMany(x => x.Transactions)
                .HasForeignKey(x => x.SendId);
        }
    }
}
