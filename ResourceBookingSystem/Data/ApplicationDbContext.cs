using Microsoft.EntityFrameworkCore;
using ResourceBookingSystem.Models;

namespace ResourceBookingSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Resource> Resources { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the one-to-many relationship
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Resource)
                .WithMany(r => r.Bookings)
                .HasForeignKey(b => b.ResourceId);

            // Seed initial data
            modelBuilder.Entity<Resource>().HasData(
                new Resource
                {
                    Id = 1,
                    Name = "Conference Room A",
                    Description = "Main conference room with 4K projector",
                    Location = "Floor 3, West Wing",
                    Capacity = 20,
                    IsAvailable = true
                },
                new Resource
                {
                    Id = 2,
                    Name = "Company Van",
                    Description = "7-seater passenger van",
                    Location = "Parking Lot B",
                    Capacity = 7,
                    IsAvailable = true
                }
            );
        }
    }
}