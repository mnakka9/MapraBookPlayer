using Microsoft.EntityFrameworkCore;

namespace MapraBookPlayer.Domain.Context
{
#nullable disable
    public class BookContext : DbContext
    {
        protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={Constants.GetDbPath()}");
        }

        public DbSet<AudioBook> Books { get; set; }
        public DbSet<Chapter> Chapters { get; set; }

        public DbSet<Bookmark> Bookmarks { get; set; }
    }
#nullable enable
}
