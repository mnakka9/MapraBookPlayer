using System.ComponentModel.DataAnnotations;

namespace MapraBookPlayer.Domain
{
#nullable disable
    public class Chapter
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ChapterName { get; set; }
        [Required]
        public string Path { get; set; }

        public ICollection<Bookmark> Bookmarks { get; set; } = [];
    }
#nullable enable
}
