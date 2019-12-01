using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Context.Configuration
{
    internal class AlbumConfiguration : IEntityTypeConfiguration<Album>
    {
        public void Configure(EntityTypeBuilder<Album> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Title)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(a => a.Description)
                .HasMaxLength(1000);
        }
    }
}