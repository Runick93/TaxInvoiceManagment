using Microsoft.EntityFrameworkCore;
using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.Persistence
{
    public class TaxInvoiceManagmentDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Asset> Assets { get; set; } = null!;
        public DbSet<Home> Homes { get; set; } = null!;
        public DbSet<Vehicle> Vehicles { get; set; } = null!;
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
                entity.Property(e => e.Address).HasMaxLength(255);
                entity.Property(e => e.VehicleNumberPlate).HasMaxLength(20);

                entity.HasOne(e => e.User)
                      .WithMany(u => u.Assets)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Home
            modelBuilder.Entity<Home>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Type).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Address).HasMaxLength(255);

                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Vehicle
            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Type).IsRequired().HasMaxLength(50);
                entity.Property(e => e.VehicleNumberPlate).HasMaxLength(20);

                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // TaxOrService
            modelBuilder.Entity<TaxOrService>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.AssetId)
                      .IsRequired(false);

                entity.HasOne(e => e.Asset)
                      .WithMany(a => a.TaxesOrServices)
                      .HasForeignKey(e => e.AssetId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Home)
                      .WithMany(h => h.TaxesOrServices)
                      .HasForeignKey(e => e.HomeId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Vehicle)
                      .WithMany(v => v.TaxesOrServices)
                      .HasForeignKey(e => e.VehicleId)
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
