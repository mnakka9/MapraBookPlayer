using System.Collections.Generic;

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

using MapraBookPlayer.Domain;
using MapraBookPlayer.Domain.Context;
using MapraBookPlayer.ReactiveUI.Mapper;
using MapraBookPlayer.ReactiveUI.ViewModels;
using MapraBookPlayer.ReactiveUI.Views;

using Microsoft.EntityFrameworkCore;

namespace MapraBookPlayer.ReactiveUI
{
    public partial class App : Application
    {
        public override void Initialize ()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted ()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var viewModel = new MainWindowViewModel();

                using BookContext bookContext = new();
                bookContext.Database.EnsureCreated();

                List<AudioBook> books = [.. bookContext.Books.Include(x => x.Chapters).AsNoTracking()];

                if (books.Count != 0)
                {
                    viewModel.AudioBooks = AudioBookMapper.MapToModel(books);
                }

                desktop.MainWindow = new MainWindow
                {
                    DataContext = viewModel,
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}