using BookingService.Booking.Domain.Bookings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingService.Booking.Persistence.Configurations
{
    public class BookingAggregateConfiguration : IEntityTypeConfiguration<BookingAggregate>
    {
        public void Configure(EntityTypeBuilder<BookingAggregate> builder)
        {
            builder.ToTable("bookings");


            builder.HasKey(x => x.Id)
                .HasName("pk_bookings");

            builder.Property(x => x.CatalogRequestId)
                .HasColumnName("catalog_request_id")
                .HasColumnType("uuid");

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasColumnType("bigint");


            builder.Property(x => x.Status)
                .HasColumnName("status")
                .HasColumnType("int");


            builder.Property(x => x.IdUser)
                .HasColumnName("user_id")
                .HasColumnType("bigint");


            builder.Property(x => x.IdBooking)
                .HasColumnName("resource_id")
                .HasColumnType("bigint");


            builder.Property(x => x.StartBooking)
                .HasColumnName("start_date")
                .HasColumnType("date");


            builder.Property(x => x.EndBooking)
                .HasColumnName("end_date")
                .HasColumnType("date");


            builder.Property(x => x.CreationBooking)
                .HasColumnName("created_at_date_time")
                .HasColumnType("timestamptz");
        }
    }
}
