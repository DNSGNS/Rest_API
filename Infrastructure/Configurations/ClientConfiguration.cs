using Microsoft.EntityFrameworkCore;
using Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class ClientConfiguration:IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable("clients");

            builder.HasKey(c => c.id);

            builder.Property(c => c.id)
                .ValueGeneratedOnAdd(); // Эквивалент SERIAL (автоинкремент)

            builder.Property(c => c.first_name)
                .IsRequired()
                .HasColumnType("varchar");

            builder.Property(c => c.second_name)
                .IsRequired()
                .HasColumnType("varchar");

            builder.Property(c => c.birth_date)
                .IsRequired()   
                .HasColumnType("timestamp with time zone");

        }
    }
}
