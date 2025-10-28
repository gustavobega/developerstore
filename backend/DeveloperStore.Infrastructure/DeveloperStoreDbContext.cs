using DeveloperStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Infrastructure
{
    public class DeveloperStoreDbContext : DbContext
    {
        public DeveloperStoreDbContext(DbContextOptions<DeveloperStoreDbContext> options)
            : base(options)
        {
        }

        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleItem> SaleItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sale>(entity =>
            {
                entity.HasKey(s => s.Id);

                entity.Property(s => s.SaleNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(s => s.Customer)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(s => s.Branch)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(s => s.TotalAmount)
                    .HasColumnType("decimal(18,2)");

                entity.Property(s => s.Date)
                    .IsRequired();

                entity.HasMany(s => s.Items)
                      .WithOne(i => i.Sale)
                      .HasForeignKey(i => i.SaleId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<SaleItem>(entity =>
            {
                entity.HasKey(i => i.Id);

                entity.Property(i => i.Product)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(i => i.Quantity)
                    .IsRequired();

                entity.Property(i => i.UnitPrice)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                entity.Property(i => i.Discount)
                    .HasColumnType("decimal(5,2)")
                    .IsRequired();

                entity.Property(i => i.Total)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
            });
        }
    }
}
