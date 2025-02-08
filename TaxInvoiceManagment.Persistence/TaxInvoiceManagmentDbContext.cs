using Microsoft.EntityFrameworkCore;
using TaxInvoiceManagment.Domain.Models;

namespace TaxInvoiceManagment.Persistence
{
    public class TaxInvoiceManagmentDbContext:DbContext
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
                entity.Property(e => e.Type).IsRequired();
                entity.HasOne(e => e.User)
                      .WithMany(u => u.Assets)
                      .HasForeignKey(e => e.UserId);
            });

            // TaxOrService
            modelBuilder.Entity<TaxOrService>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
                entity.HasOne(e => e.Asset)
                      .WithMany(a => a.TaxesOrServices)
                      .HasForeignKey(e => e.AssetId);
            });

            // Invoice
            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PdfPath).IsRequired();
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
                entity.HasOne(e => e.TaxOrService)
                      .WithMany(t => t.Invoices)
                      .HasForeignKey(e => e.TaxOrServiceId);
            });
        }
    }
}
