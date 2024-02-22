using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using Avalonia.Media.Imaging;

using MapraBookPlayer.Domain;
using MapraBookPlayer.ReactiveUI.ViewModels;

namespace MapraBookPlayer.ReactiveUI.Mapper
{
    public static class AudioBookMapper
    {
        public static AudioBookViewModel MapToModel (AudioBook audioBook)
        {
            return new AudioBookViewModel
            {
                Id = audioBook.Id,
                Title = audioBook.Title,
                Author = audioBook.Author,
                Cover = audioBook.Cover,
                Chapters = audioBook.Chapters,
                Description = audioBook.Description,
                Image = !string.IsNullOrEmpty(audioBook.Cover) ? new Bitmap(audioBook.Cover) : LoadImageAsync("https://placehold.co/200X180").Result,
            };
        }

        public static List<AudioBookViewModel> MapToModel (List<AudioBook> audioBooks)
        {
            return audioBooks.ConvertAll(x => MapToModel(x));
        }

        private static async Task<Bitmap?> LoadImageAsync (string imageUrl)
        {
            try
            {
                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(imageUrl);
                response.EnsureSuccessStatusCode();
                using var stream = await response.Content.ReadAsStreamAsync();

                return new Bitmap(stream);
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                Console.WriteLine($"Error loading image: {ex.Message}");
            }

            return default;
        }
    }
}
