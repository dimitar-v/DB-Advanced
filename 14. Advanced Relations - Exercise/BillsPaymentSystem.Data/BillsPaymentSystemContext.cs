namespace BillsPaymentSystem.Data
{
    using EntityConfigurations;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class BillsPaymentSystemContext : DbContext
    {
        public BillsPaymentSystemContext()
        { }

        public BillsPaymentSystemContext(DbContextOptions options)
            : base(options)
        { }

        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Config.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new BankAccountConfig());
            //modelBuilder.ApplyConfiguration(new CreditCardConfig());
            //modelBuilder.ApplyConfiguration(new PaymentMethodConfig());
        }
    }
}
