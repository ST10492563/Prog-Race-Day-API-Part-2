
using Microsoft.EntityFrameworkCore;
using RaceDayApi.Models;

namespace RaceDayApi.Data
{
    public class RaceDayDbContext : DbContext
    {
        public RaceDayDbContext(
            DbContextOptions<RaceDayDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<EventCategory> EventCategories { get; set; }

        // FIXED: Use your Route model
        public DbSet<RaceDayApi.Models.Route> Routes { get; set; }

        public DbSet<EventWeather> EventWeather { get; set; }

        public DbSet<EventEnrollment> EventEnrollments { get; set; }

        public DbSet<Result> Results { get; set; }


        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // EventCategory composite primary key

            modelBuilder.Entity<EventCategory>()
                .HasKey(ec => new
                {
                    ec.EventID,
                    ec.CategoryID
                });


            // Event -> Organiser

            modelBuilder.Entity<Event>()
                .HasOne(e => e.Organiser)
                .WithMany()
                .HasForeignKey(e => e.OrganiserID)
                .OnDelete(DeleteBehavior.Restrict);


            // EventCategory -> Event

            modelBuilder.Entity<EventCategory>()
                .HasOne(ec => ec.Event)
                .WithMany(e => e.EventCategories)
                .HasForeignKey(ec => ec.EventID)
                .OnDelete(DeleteBehavior.Cascade);


            // EventCategory -> Category

            modelBuilder.Entity<EventCategory>()
                .HasOne(ec => ec.Category)
                .WithMany(c => c.EventCategories)
                .HasForeignKey(ec => ec.CategoryID)
                .OnDelete(DeleteBehavior.Cascade);


            // Event -> Route
            // FIXED: Use your Route model

            modelBuilder.Entity<RaceDayApi.Models.Route>()
                .HasOne(r => r.Event)
                .WithMany(e => e.Routes)
                .HasForeignKey(r => r.EventID)
                .OnDelete(DeleteBehavior.Cascade);


            // Event -> Weather

            modelBuilder.Entity<EventWeather>()
                .HasOne(w => w.Event)
                .WithMany(e => e.Weather)
                .HasForeignKey(w => w.EventID)
                .OnDelete(DeleteBehavior.Cascade);


            // Enrollment -> Event

            modelBuilder.Entity<EventEnrollment>()
                .HasOne(en => en.Event)
                .WithMany(e => e.EventEnrollments)
                .HasForeignKey(en => en.EventID)
                .OnDelete(DeleteBehavior.Cascade);


            // Enrollment -> Participant

            modelBuilder.Entity<EventEnrollment>()
                .HasOne(en => en.Participant)
                .WithMany()
                .HasForeignKey(en => en.ParticipantID)
                .OnDelete(DeleteBehavior.Restrict);


            // Enrollment -> Category

            modelBuilder.Entity<EventEnrollment>()
                .HasOne(en => en.Category)
                .WithMany()
                .HasForeignKey(en => en.CategoryID)
                .OnDelete(DeleteBehavior.Restrict);


            // Enrollment -> Result

            modelBuilder.Entity<Result>()
                .HasOne(r => r.Enrollment)
                .WithOne(e => e.Result)
                .HasForeignKey<Result>(r => r.EnrollmentID)
                .OnDelete(DeleteBehavior.Cascade);


            // Unique email

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();


            // One participant cannot enrol twice
            // in the same event

            modelBuilder.Entity<EventEnrollment>()
                .HasIndex(e => new
                {
                    e.EventID,
                    e.ParticipantID
                })
                .IsUnique();
        }
    }
}


