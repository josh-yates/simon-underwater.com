using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Context.Configuration
{
    internal class CustomContentConfiguration : IEntityTypeConfiguration<CustomContent>
    {
        public void Configure(EntityTypeBuilder<CustomContent> builder)
        {
            builder.HasKey(cc => cc.Id);
            builder.Property(cc => cc.Key)
                .IsRequired();
        }
    }
}