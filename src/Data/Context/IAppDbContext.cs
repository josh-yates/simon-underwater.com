using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public interface IAppDbContext
    {
        DbSet<Album> Albums { get; set; }
        DbSet<AlbumImage> AlbumImages { get; set; }
        DbSet<ContactForm> ContactForms { get; set; }
        DbSet<CustomContent> CustomContent { get; set; }
        DbSet<DailySiteView> DailySiteViews { get; set; }
        DbSet<Image> Images { get; set; }
        DbSet<Log> Logs { get; set; }
        DbSet<User> Users { get; set; }
    }
}