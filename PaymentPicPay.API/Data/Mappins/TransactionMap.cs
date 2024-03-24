using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentPicPay.API.Domain.Models;

namespace PaymentPicPay.API.Data.Mappins
{
    public class TransactionMap : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transfers");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.SendId)
                .HasColumnName("SendUserId")
                .IsRequired();

            builder.Property(x => x.ReceiveId)
                .HasColumnName("ReceivedUserId")
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
        }
    }
}
