using Microsoft.EntityFrameworkCore;
using Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations

{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("orders");
            builder.HasKey(o => o.id);

            builder.Property(o => o.id)
              .UseIdentityAlwaysColumn();


            builder.Property(o => o.amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(o => o.status)
                .HasConversion<string>()
                .HasColumnType("varchar(50)")
                .IsRequired();

            // CHECK-ограничение на допустимые значения статуса
            builder.HasCheckConstraint("CK_orders_status_valid",
                "status IN ('notprocessed', 'completed', 'cancelled')");

            builder.Property(o => o.order_datetime)
              .HasColumnType("timestamp with time zone")
              .IsRequired();

            
            builder.HasOne(o => o.client)
                .WithMany()
                .HasForeignKey(o => o.client_id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
