using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Album> Albums { get; set; }
        public DbSet<AlbumImage> AlbumImages { get; set; }
    }
}