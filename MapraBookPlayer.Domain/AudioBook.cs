using System.ComponentModel.DataAnnotations;

namespace MapraBookPlayer.Domain
{
#nullable disable
    public class AudioBook
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string Cover { get; set; }

        public ICollection<Chapter> Chapters { get; set; } = [];
    }
#nullable enable
}
