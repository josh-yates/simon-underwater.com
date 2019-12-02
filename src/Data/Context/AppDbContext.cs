using Data.Context.Configuration;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public DbSet<Album> Albums { get; set; }
        public DbSet<AlbumImage> AlbumImages { get; set; }
        public DbSet<ContactForm> ContactForms { get; set; }
        public DbSet<CustomContent> CustomContent { get; set; }
        public DbSet<DailySiteView> DailySiteViews { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new AlbumConfiguration());
            builder.ApplyConfiguration(new AlbumImageConfiguration());
            builder.ApplyConfiguration(new ContactFormConfiguration());
            builder.ApplyConfiguration(new CustomContentConfiguration());
            builder.ApplyConfiguration(new DailySiteViewConfiguration());
            builder.ApplyConfiguration(new ImageConfiguration());
            builder.ApplyConfiguration(new LogConfiguration());
            builder.ApplyConfiguration(new UserConfiguration());
        }
    }
}