using BookingService.Booking.Domain.Bookings;
using BookingService.Booking.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Booking.Persistence
{
    public class BookingsContext : DbContext
    {
        public DbSet<BookingAggregate> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   
            modelBuilder.ApplyConfiguration<BookingAggregate>(new BookingAggregateConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
