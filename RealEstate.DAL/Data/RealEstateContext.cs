using Microsoft.EntityFrameworkCore;
using RealEstate.DAL.Models;

namespace RealEstate.DAL
{
    public class RealEstateContext : DbContext
    {
        public RealEstateContext(DbContextOptions<RealEstateContext> options) : base(options) { }

        public DbSet<Client> Clients { get; set; } = null!;
        public DbSet<RealEstateProperty> RealEstateProperties { get; set; } = null!;
        public DbSet<Offer> Offers { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Offer>()
                .HasMany(o => o.RealEstateProperties)
                .WithMany(re => re.Offers)
                .UsingEntity(j => j.ToTable("OfferRealEstateProperty"));

            // Seed data for testing
            modelBuilder.Entity<Client>().HasData(
                new Client { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@email.com", Phone = "+1234567890", BankAccount = "UA1234567890" },
                new Client { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@email.com", Phone = "+0987654321", BankAccount = "UA0987654321" }
            );

            modelBuilder.Entity<RealEstateProperty>().HasData(
                new RealEstateProperty { Id = 1, Type = RealEstateType.OneRoomApartment, Address = "123 Main St", Price = 50000, Area = 45.5, Description = "Cozy one-room apartment" },
                new RealEstateProperty { Id = 2, Type = RealEstateType.TwoRoomApartment, Address = "456 Oak St", Price = 75000, Area = 65.0, Description = "Spacious two-room apartment" }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}