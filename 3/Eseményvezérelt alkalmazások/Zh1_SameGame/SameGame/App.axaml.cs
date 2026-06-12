using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using SameGame.Models;
using SameGame.ViewModels;
using SameGame.Views;

namespace SameGame
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var model = new GameModel();
                var ViewModel = new MainWindowViewModel(model);

                desktop.MainWindow = new MainWindow
                {
                    DataContext = ViewModel,
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}