using System;
using System.Linq;

using Avalonia.Controls;
using Avalonia.Interactivity;

using MapraBookPlayer.Domain;
using MapraBookPlayer.Domain.Context;
using MapraBookPlayer.ReactiveUI.ViewModels;

using Microsoft.EntityFrameworkCore;

namespace MapraBookPlayer.ReactiveUI.Views
{
    public partial class Player : Window
    {
        public Player ()
        {
            InitializeComponent();
        }

        private void Chapters_SelectionChanged (object sender, SelectionChangedEventArgs e)
        {
            // Handle selection changed event here
            var listBox = (ListBox)sender;
            Chapter? selectedItem = (Chapter?)listBox.SelectedItem;
            if (selectedItem != null)
            {
                var chapter = selectedItem as Chapter;
                var viewModel = (PlayerViewModel)DataContext!;
                using BookContext context = new();
                chapter.Bookmarks = [.. context.Bookmarks.Where(x => x.ChapterId == chapter.Id)];
                viewModel.CurrentChapter = chapter;
                viewModel.StopPlaying();
                viewModel.StartPlaying();
            }
        }

        private void Bookmarks_SelectionChanged (object sender, SelectionChangedEventArgs e)
        {
            // Handle selection changed event here
            var listBox = (ListBox)sender;
            Bookmark? selectedItem = (Bookmark?)listBox.SelectedItem;
            if (selectedItem != null && DataContext is PlayerViewModel viewModel)
            {
                var chapterList = viewModel.AudioBook!.Chapters.Select(x => x.Id).ToList();

                viewModel.SelectedIndex = chapters.SelectedIndex = chapterList.IndexOf(selectedItem.ChapterId);

                viewModel.Position = selectedItem.Position;

                lblMark.Content = selectedItem.Mark;

                if (!string.IsNullOrEmpty(selectedItem.Header))
                {
                    txtHeader.Text = selectedItem.Header;
                }

                if (!string.IsNullOrEmpty(selectedItem.Description))
                {
                    txtDescription.Text = selectedItem.Description;
                }

                btnEditBookmark.IsVisible = true;
                btnDeleteBookmark.IsVisible = true;
            }
        }

        protected override void OnLoaded (RoutedEventArgs e)
        {
            if (DataContext is PlayerViewModel viewModel)
            {
                chapters.ItemsSource = viewModel.AudioBook!.Chapters.ToList();
                chapters.SelectedIndex = 0;

                bookmarksList.ItemsSource = viewModel.Bookmarks;
            }

            base.OnLoaded(e);
        }

        private void Slider_ValueChanged (object? sender, Avalonia.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            var slider = (Slider)sender!;
            var _viewModel = (PlayerViewModel)DataContext!;
            _viewModel.SeekCommand.Execute(slider.Value);
        }

        protected override void OnUnloaded (RoutedEventArgs e)
        {
            if (DataContext is PlayerViewModel viewModel)
            {
                viewModel.StopCommand.Execute().Subscribe();
            }

            base.OnUnloaded(e);
        }

        private void AddBookmark_Click (object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (DataContext is PlayerViewModel viewModel)
            {
                var currentChapter = viewModel.CurrentChapter;

                if (currentChapter != null)
                {
                    using BookContext context = new();
                    Bookmark mark = new()
                    {
                        Chapter = currentChapter,
                        ChapterId = currentChapter.Id,
                        Position = viewModel.Position
                    };

                    currentChapter.Bookmarks ??= [];
                    currentChapter.Bookmarks.Add(mark);
                    context.Chapters.Update(currentChapter);
                    context.SaveChanges();

                    viewModel.Bookmarks.Add(mark);
                }
            }
        }

        private async void ButtonEditBookmark_Click (object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (bookmarksList.SelectedItem is Bookmark bookmark)
            {
                var model = new EditBookmarkViewModel
                {
                    Bookmark = bookmark
                };

                var window = new EditBookmark
                {
                    DataContext = model
                };

                await window.ShowDialog(this);

                await using var context = new BookContext();
                var getBookmark = context.Bookmarks.First(x => x.Id == bookmark.Id);

                bookmark.Header = getBookmark.Header;
                bookmark.Description = getBookmark.Description;

                txtHeader.Text = getBookmark.Header;
                txtDescription.Text = getBookmark.Description;
            }
        }

        private void ButtonDeleteBookmark_Click (object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (bookmarksList.SelectedItem is Bookmark bookmark)
            {
                var viewModel = (PlayerViewModel)DataContext!;

                viewModel.Bookmarks.Remove(bookmark);

                using var context = new BookContext();
                var chapter = context.Chapters.Include(x => x.Bookmarks).First(x => x.Id == bookmark.ChapterId);

                chapter.Bookmarks.Remove(chapter.Bookmarks.First(x => x.Id == bookmark.Id));
                context.Chapters.Update(chapter);
                context.SaveChanges();

                txtHeader.Text = string.Empty;
                txtDescription.Text = string.Empty;

                btnEditBookmark.IsVisible = false;
                btnDeleteBookmark.IsVisible = false;
                lblMark.Content = string.Empty;
            }
        }
    }
}
