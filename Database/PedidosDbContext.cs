using System.Collections.Generic;
using Guren.Model;
using Microsoft.EntityFrameworkCore;

namespace Guren.Database
{
    public partial class PedidosDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<SeasonalCampaign> SeasonalCampaigns { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        public PedidosDbContext() { }

        public PedidosDbContext(DbContextOptions<PedidosDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#warning Para proteger sua connection string, mova-a para o appsettings.json ou secrets
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(
                    "server=localhost;user id=root;password=1234;database=guren",
                    ServerVersion.Parse("8.1.0-mysql")
                );
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
