using MapraBookPlayer.Domain;

using ReactiveUI;

namespace MapraBookPlayer.ReactiveUI.ViewModels
{
    public class EditBookmarkViewModel : ViewModelBase
    {
        private Bookmark bookmark;

        public Bookmark Bookmark
        {
            get => bookmark;
            set => this.RaiseAndSetIfChanged(ref bookmark, value);
        }

        public EditBookmarkViewModel ()
        {
            bookmark = new Bookmark();
        }
    }
}
