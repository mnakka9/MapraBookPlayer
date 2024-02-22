using System.Collections.Generic;

using Avalonia.Media.Imaging;

using MapraBookPlayer.Domain;

namespace MapraBookPlayer.ReactiveUI.ViewModels
{
    public class AudioBookViewModel
    {
        public int Id { get; set; }

        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Author { get; set; }
        public string? Cover { get; set; }

        public Bitmap? Image { get; set; }

        public ICollection<Chapter> Chapters { get; set; } = [];
    }
}
