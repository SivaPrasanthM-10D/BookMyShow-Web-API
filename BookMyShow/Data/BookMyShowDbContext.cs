using BookMyShow.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookMyShow.Data
{
    public class BookMyShowDbContext : DbContext
    {
        public BookMyShowDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Theatre> Theatres { get; set; }
        public DbSet<TheatreOwner> TheatreOwners { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Screen> Screens { get; set; }
        public DbSet<Show> Shows { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<MovieReview> MovieReviews { get; set; }
        public DbSet<Admin> Admins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Show>()
                .Property(s => s.TicketPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Ticket>()
                .Property(t => t.TicketPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Theatre>()
                .HasOne(t => t.TheatreOwner)
                .WithMany()
                .HasForeignKey(t => t.TheatreOwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Screen>()
                .HasOne(s => s.Theatre)
                .WithMany(t => t.Screens)
                .HasForeignKey(s => s.TheatreId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Show>()
                .HasOne(sh => sh.Screen)
                .WithMany(s => s.Shows)
                .HasForeignKey(sh => sh.ScreenId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Show>()
                .HasOne(sh => sh.Movie)
                .WithMany()
                .HasForeignKey(sh => sh.MovieId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Customer)
                .WithMany()
                .HasForeignKey(t => t.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Show)
                .WithMany()
                .HasForeignKey(t => t.ShowId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MovieReview>()
                .HasOne(mr => mr.User)
                .WithMany()
                .HasForeignKey(mr => mr.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MovieReview>()
                .HasOne(mr => mr.Movie)
                .WithMany()
                .HasForeignKey(mr => mr.MovieId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Movie>()
                .HasOne<Admin>()
                .WithMany(a => a.Movies)
                .HasForeignKey(m => m.AdminId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}