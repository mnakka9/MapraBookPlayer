using Avalonia.Controls;
using Avalonia.Interactivity;

using MapraBookPlayer.Domain;
using MapraBookPlayer.Domain.Context;
using MapraBookPlayer.ReactiveUI.ViewModels;

namespace MapraBookPlayer.ReactiveUI;

public partial class EditBookmark : Window
{
    private Bookmark? Bookmark { get; set; }

    public EditBookmark ()
    {
        InitializeComponent();
    }

    private void SaveBookmark_Click (object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (Bookmark != null)
        {
            Bookmark.Header = txtHeader.Text;
            Bookmark.Description = txtDesc.Text;

            using BookContext context = new();
            context.Bookmarks.Update(Bookmark);
            context.SaveChanges();

            this.Close();
        }
    }

    private void Cancel_Click (object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.Close();
    }

    protected override void OnLoaded (RoutedEventArgs e)
    {
        if (DataContext is EditBookmarkViewModel model)
        {
            if (model.Bookmark != null)
            {
                txtHeader.Text = model.Bookmark.Header;
                txtDesc.Text = model.Bookmark.Description;
                Bookmark = model.Bookmark;
            }
        }

        base.OnLoaded(e);
    }
}