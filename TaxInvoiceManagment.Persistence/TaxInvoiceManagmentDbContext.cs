using Microsoft.EntityFrameworkCore;
using TaxInvoiceManagment.Domain.Entities;

namespace TaxInvoiceManagment.Persistence
{
    // dotnet ef migrations add InitialCreate --project TaxInvoiceManagment.Persistence --startup-project TaxInvoiceManagment.Presentation.Api
    public class TaxInvoiceManagmentDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<TaxableItem> TaxableItems { get; set; } = null!;
        public DbSet<Tax> TaxesOrServices { get; set; } = null!;
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
                entity.Property(e => e.UserName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(255);
            });

            // Taxable Item
            modelBuilder.Entity<TaxableItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Type).IsRequired();
                entity.Property(e => e.Type).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Address).HasMaxLength(255);
                entity.Property(e => e.VehicleNumberPlate).HasMaxLength(20);

                entity.HasOne(e => e.User)
                      .WithMany(u => u.TaxableItems)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Tax
            modelBuilder.Entity<Tax>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TaxableItemId).IsRequired();

                entity.HasOne(e => e.TaxableItem)
                      .WithMany(a => a.Taxes)
                      .HasForeignKey(e => e.TaxableItemId)
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

                entity.HasOne(e => e.Tax)
                      .WithMany(t => t.Invoices)
                      .HasForeignKey(e => e.TaxId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
