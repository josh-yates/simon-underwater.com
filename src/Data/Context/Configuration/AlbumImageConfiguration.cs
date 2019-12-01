using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Context.Configuration
{
    internal class AlbumImageConfiguration : IEntityTypeConfiguration<AlbumImage>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<AlbumImage> builder)
        {
            builder.HasKey(ai => new { ai.AlbumId, ai.ImageId });
            builder.HasOne(ai => ai.Album)
                .WithMany(a => a.AlbumImages)
                .HasForeignKey(ai => ai.AlbumId);
            builder.HasOne(ai => ai.Image)
                .WithMany(i => i.ImageAlbums)
                .HasForeignKey(ai => ai.ImageId);
        }
    }
}