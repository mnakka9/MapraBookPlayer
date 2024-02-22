using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MapraBookPlayer.Domain
{
#nullable disable
    public class Bookmark
    {
        [Key]
        public int Id { get; set; }
        public string Header { get; set; }
        public string Description { get; set; }

        [Required]
        public int ChapterId { get; set; }

        [ForeignKey("ChapterId")]
        public Chapter Chapter { get; set; }

        public double Position { get; set; }

        [NotMapped]
        public string Mark
        {
            get => $"{TimeSpan.FromSeconds(Position):mm\\:ss} - {Chapter.ChapterName}";
        }
    }
#nullable enable
}
