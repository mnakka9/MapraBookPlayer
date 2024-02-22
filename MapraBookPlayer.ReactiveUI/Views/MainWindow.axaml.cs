using System.Collections.Generic;
using System.IO;
using System.Linq;

using Avalonia.Controls;

using MapraBookPlayer.Domain;
using MapraBookPlayer.Domain.Context;
using MapraBookPlayer.ReactiveUI.Mapper;
using MapraBookPlayer.ReactiveUI.ViewModels;

using Microsoft.EntityFrameworkCore;

namespace MapraBookPlayer.ReactiveUI.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow ()
        {
            InitializeComponent();
        }

        private async void Image_PointerPressed (object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            if (sender is Image image && image.DataContext is AudioBookViewModel bookViewModel)
            {
                List<int> chapterIds = [.. bookViewModel.Chapters.Select(x => x.Id)];
                await using var context = new BookContext();
                var bookmarks = context.Bookmarks.Include(x => x.Chapter).Where(x => chapterIds.Contains(x.ChapterId)).AsNoTracking().ToList();

                var model = new PlayerViewModel
                {
                    AudioBook = bookViewModel,
                };

                bookmarks.ForEach(x => model.Bookmarks.Add(x));

                var playerWindow = new Player()
                {
                    DataContext = model
                };

                await playerWindow.ShowDialog(this);
            }
        }

        private async void AddNew_Click (object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var dialog = this.VisualRoot as Window;

            if (dialog is null)
            {
                return;
            }

            var folders = await dialog.StorageProvider.OpenFolderPickerAsync(new()
            {
                Title = "Select an Audio book folder"
            });

            if (folders != null)
            {
                var selectedFolder = folders.First();

                AudioBook book = new()
                {
                    Title = selectedFolder.Name,
                };

                bool addDetails = false;

                await foreach (var innerItem in selectedFolder.GetItemsAsync())
                {
                    FileInfo info = new(innerItem.Path.LocalPath);

                    if (info.Extension == ".jpg" || info.Extension == ".png" || info.Extension == ".jpeg")
                    {
                        book.Cover = innerItem.Path.LocalPath;
                        continue;
                    }
                    else
                    {
                        if (!addDetails)
                        {
                            var tags = TagLib.File.Create(innerItem.Path.LocalPath);

                            book.Title = tags.Tag.Album ?? tags.Tag.Title ?? book.Title;
                            book.Author = tags.Tag.FirstAlbumArtist ?? tags.Tag.FirstPerformer;
                            book.Description = tags.Tag.Comment;

                            addDetails = true;
                        }
                    }

                    if (info.Extension == ".mp3"
                        || info.Extension == ".m4a"
                        || info.Extension == ".flac"
                        || info.Extension == ".m4b"
                        || info.Extension == ".wav"
                        || info.Extension == ".aac")
                    {
                        var tags = TagLib.File.Create(innerItem.Path.LocalPath);

                        Chapter chapter = new()
                        {
                            ChapterName = tags.Tag.Title ?? innerItem.Name,
                            Path = innerItem.Path.LocalPath
                        };

                        book.Chapters.Add(chapter);
                    }
                }

                await using var context = new BookContext();

                context.Books.Add(book);
                await context.SaveChangesAsync();

                if (DataContext is MainWindowViewModel viewModel)
                {
                    viewModel.AudioBooks.Add(AudioBookMapper.MapToModel(book));
                }
            }
        }
    }
}