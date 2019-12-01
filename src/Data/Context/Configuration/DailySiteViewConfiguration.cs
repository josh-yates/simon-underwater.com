using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Context.Configuration
{
    internal class DailySiteViewConfiguration : IEntityTypeConfiguration<DailySiteView>
    {
        public void Configure(EntityTypeBuilder<DailySiteView> builder)
        {
            builder.HasKey(dsv => dsv.Id);
        }
    }
}