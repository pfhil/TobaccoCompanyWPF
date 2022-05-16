using Microsoft.EntityFrameworkCore;
using TobaccoCompanyWPF.Models.Entity;
using TobaccoCompanyWPF.ViewModels.Services;

namespace TobaccoCompanyWPF.Models
{
    internal class TobaccoCompanyContext : DbContext
    {
        public static string DbConnStr = ConfigurationService.ConnectionString;

        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Consignment> Consignments => Set<Consignment>();
        public DbSet<CashReceipt> CashReceipts => Set<CashReceipt>();
        public DbSet<Invoice> Invoices => Set<Invoice>();

        public TobaccoCompanyContext(bool create = false, bool delete = false)
        {
            if (delete)
            {
                Database.EnsureDeleted();
            }
            if (create)
            {
                Database.EnsureCreated();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            optionsBuilder.UseSqlServer(DbConnStr);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Order>()
                .HasMany(o => o.Products)
                .WithMany(p => p.Orders)
                .UsingEntity<CashReceipt> //Many Order to Many Product
                (
                    cashReceiptTypeBuilder => cashReceiptTypeBuilder //Many CashReceipt to One Product
                    .HasOne(cr => cr.Product)
                    .WithMany(p => p.CashReceipts)
                    .HasForeignKey(cr => cr.ProductId),

                    cashReceiptTypeBuilder => cashReceiptTypeBuilder //Many CashReceipt to One Order
                    .HasOne(cr => cr.Order)
                    .WithMany(o => o.CashReceipts)
                    .HasForeignKey(cr => cr.OrderId)
                    );

            modelBuilder
                .Entity<Consignment>()
                .HasMany(c => c.Products)
                .WithMany(p => p.Consignments)
                .UsingEntity<Invoice> //Many Consignment to Many Product
                (
                    invoiceTypeBuilder => invoiceTypeBuilder //Many Invoice to One Product
                    .HasOne(i => i.Product)
                    .WithMany(p => p.Invoices)
                    .HasForeignKey(i => i.ProductId),

                    invoiceTypeBuilder => invoiceTypeBuilder //Many Invoice to One Consignment
                    .HasOne(i => i.Consignment)
                    .WithMany(c => c.Invoices)
                    .HasForeignKey(i => i.ConsignmentId)
                    );

        }
    }
}
