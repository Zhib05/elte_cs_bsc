using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;

using mintaZh_Invazio_Avalonia.Model;
using mintaZh_Invazio_Avalonia.ViewModels;
using mintaZh_Invazio_Avalonia.Views;

namespace mintaZh_Invazio_Avalonia;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Csak akkor indulunk el, ha asztali környezetben vagyunk
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // 1. Modell létrehozása (a SudokuTimerInheritance osztályt használva)
            var gameModel = new InvasionGameModel(new SudokuTimerInheritance());

            // 2. ViewModel létrehozása
            var viewModel = new InvasionViewModel(gameModel);

            // 3. Ablak létrehozása és összekötés
            desktop.MainWindow = new MainWindow
            {
                DataContext = viewModel
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
