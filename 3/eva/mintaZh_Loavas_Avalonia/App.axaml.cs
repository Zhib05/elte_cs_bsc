using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;

using mintaZh_Loavas_Avalonia.ViewModels;
using mintaZh_Loavas_Avalonia.Views;
using mintaZh_Loavas_Avalonia.Model;
using System;
using Avalonia.Threading;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace mintaZh_Loavas_Avalonia;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var gameModel = new GameModel();
            gameModel.GameOver += Model_GameOver;

            var viewModel = new MainViewModel(gameModel);
            desktop.MainWindow = new MainWindow
            {
                DataContext = viewModel
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void Model_GameOver(object? sender, GameModel.GameOverEventArgs e)
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            MessageBoxManager.GetMessageBoxStandard(
                "Játék vége",
                $"Játék vége!\n\nGYŐZTES: {e.Winner}\n\nGratulálunk!",
                ButtonEnum.Ok,
                Icon.Info).ShowAsync();
        });
    }
}
