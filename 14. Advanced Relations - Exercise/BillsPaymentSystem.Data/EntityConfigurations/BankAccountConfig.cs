namespace BillsPaymentSystem.Data.EntityConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class BankAccountConfig : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            builder
                .Property(b => b.BankName)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(b => b.SWIFT)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsRequired();
        }
    }
}
