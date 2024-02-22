using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

using MapraBookPlayer.Domain.Context;

namespace MapraBookPlayer
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
                desktop.MainWindow = new MainWindow();
                BookContext context = new();
                context.Database.EnsureCreated();
                context.Dispose();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}