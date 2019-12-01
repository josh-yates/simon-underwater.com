using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Album> Albums { get; set; }
        public DbSet<AlbumImage> AlbumImages { get; set; }
        public DbSet<ContactForm> ContactForms { get; set; }
        public DbSet<CustomContent> CustomContent { get; set; }
        public DbSet<DailySiteView> DailySiteViews { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<User> Users { get; set; }
    }
}