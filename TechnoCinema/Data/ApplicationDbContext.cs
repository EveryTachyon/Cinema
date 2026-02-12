using Microsoft.EntityFrameworkCore;
using TechnoCinema.Models;  // Make sure this is the namespace where your models are located

namespace TechnoCinema.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        // Define DbSets for each model (i.e., tables)
        public DbSet<Banner> Banners { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Showtime> Showtimes { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<User> Users { get; set; }
    

         public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries<Showtime>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added ||
                    entry.State == EntityState.Modified)
                {
                    entry.Entity.ModifiedAt = DateTime.Now;
                }
            }

            return base.SaveChanges();
        }

    }
}
