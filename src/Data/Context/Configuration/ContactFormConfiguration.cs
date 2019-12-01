using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Context.Configuration
{
    internal class ContactFormConfiguration : IEntityTypeConfiguration<ContactForm>
    {
        public void Configure(EntityTypeBuilder<ContactForm> builder)
        {
            builder.HasKey(cf => cf.Id);
            builder.Property(cf => cf.EmailAddress)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(cf => cf.Name)
                .IsRequired()
                .HasMaxLength(201);
            builder.Property(cf => cf.Content)
                .IsRequired()
                .HasMaxLength(5000);
        }
    }
}