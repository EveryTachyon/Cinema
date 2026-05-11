using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TechnoCinema.Models;  // Make sure this is the namespace where your models are located
using System.Text.Json;
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

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var converter = new ValueConverter<List<string>, string>(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null) ?? new List<string>());

            modelBuilder.Entity<Movie>()
                .Property(e => e.Genres)
                .HasConversion(converter);

            modelBuilder.Entity<Movie>()
                .Property(e => e.Subtitles)
                .HasConversion(converter);
        }


      

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