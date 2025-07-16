// Data/DatabaseInitializer.cs
using Microsoft.EntityFrameworkCore;
using ResourceBookingSystem.Models;

namespace ResourceBookingSystem.Data
{
    public static class DatabaseInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.Migrate(); // Apply pending migrations
            
            if (!context.Resources.Any()) // Only seed if database is empty
            {
                var resources = new List<Resource>
                {
                    new Resource 
                    {
                        Name = "Conference Room A",
                        Description = "Main meeting room with projector",
                        Location = "Floor 3, West Wing",
                        Capacity = 20,
                        IsAvailable = true
                    },
                    new Resource 
                    {
                        Name = "Company Van",
                        Description = "7-seater passenger van",
                        Location = "Parking Lot B",
                        Capacity = 7,
                        IsAvailable = true
                    }
                };

                context.Resources.AddRange(resources);
                context.SaveChanges();

                var bookings = new List<Booking>
                {
                    new Booking
                    {
                        ResourceId = 1,
                        StartTime = DateTime.Now.AddHours(2),
                        EndTime = DateTime.Now.AddHours(3),
                        BookedBy = "John Doe",
                        Purpose = "Team Meeting"
                    },
                    new Booking
                    {
                        ResourceId = 2,
                        StartTime = DateTime.Now.AddDays(1),
                        EndTime = DateTime.Now.AddDays(1).AddHours(2),
                        BookedBy = "Jane Smith",
                        Purpose = "Client Visit"
                    }
                };

                context.Bookings.AddRange(bookings);
                context.SaveChanges();
            }
        }
    }
}