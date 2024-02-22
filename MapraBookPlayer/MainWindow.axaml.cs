using System.Collections.Generic;
using System.Linq;

using Avalonia.Controls;

using MapraBookPlayer.Domain;
using MapraBookPlayer.Domain.Context;

using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace MapraBookPlayer
{
    public partial class MainWindow : Window
    {
        public List<AudioBook> AudioBooks { get; set; }

        public MainWindow ()
        {
            InitializeComponent();
            this.AddNewBookButton.Click += AddNewBookButton_Click;
            AudioBooks = GetAudioBooks();

            DataContext = AudioBooks;
        }

        private static List<AudioBook> GetAudioBooks ()
        {
            using var context = new BookContext();

            var books = context.Books.ToList();

            return books.Count != 0 ? books : [];
        }

        private async void AddNewBookButton_Click (object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var box = MessageBoxManager
          .GetMessageBoxStandard("Caption", "Are you sure you would like to delete appender_replace_page_1?",
              ButtonEnum.YesNo);

            var result = await box.ShowAsync();
        }
    }
}