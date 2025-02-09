using Microsoft.EntityFrameworkCore;
using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.Persistence
{
    public class TaxInvoiceManagmentDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Asset> Assets { get; set; } = null!;
        public DbSet<TaxOrService> TaxesOrServices { get; set; } = null!;
        public DbSet<Invoice> Invoices { get; set; } = null!;

        public TaxInvoiceManagmentDbContext(DbContextOptions<TaxInvoiceManagmentDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            });

            // Asset
            modelBuilder.Entity<Asset>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Type).IsRequired().HasMaxLength(50);
                entity.Property(e => e.StreetName).HasMaxLength(150);
                entity.Property(e => e.Department).HasMaxLength(10);
                entity.Property(e => e.VehicleNumberPlate).HasMaxLength(20);

                entity.HasOne(e => e.User)
                      .WithMany(u => u.Assets)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // TaxOrService
            modelBuilder.Entity<TaxOrService>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ServiceName).HasMaxLength(100);
                entity.Property(e => e.ServiceDescription).HasMaxLength(255);
                entity.Property(e => e.ResponsibleName).HasMaxLength(100);
                entity.Property(e => e.ServiceType).HasMaxLength(50);
                entity.Property(e => e.ClientNumber).HasMaxLength(50);
                entity.Property(e => e.AnnualPayment).IsRequired();

                entity.HasOne(e => e.Asset)
                      .WithMany(a => a.TaxesOrServices)
                      .HasForeignKey(e => e.AssetId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Invoice
            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Number).IsRequired();
                entity.Property(e => e.Month).IsRequired().HasMaxLength(20);
                entity.Property(e => e.InvoiceAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.InvoiceReceiptPath).HasMaxLength(255);
                entity.Property(e => e.PaymentReceiptPath).HasMaxLength(255);
                entity.Property(e => e.Notes).HasMaxLength(500);

                entity.HasOne(e => e.TaxOrService)
                      .WithMany(t => t.Invoices)
                      .HasForeignKey(e => e.TaxOrServiceId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
