using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Context.Configuration
{
    internal class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.OnDiskName)
                .IsRequired();
            builder.Property(i => i.OriginalName)
                .IsRequired();
            builder.Property(i => i.Description)
                .HasMaxLength(1000);
        }
    }
}