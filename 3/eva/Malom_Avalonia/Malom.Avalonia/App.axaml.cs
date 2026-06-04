using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using Malom.Avalonia.ViewModels;
using Malom.Avalonia.Views;
using Malom.Model;
using Malom.Persistence;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace Malom.Avalonia;

public partial class App : Application
{
    #region Fields

    private GameModel _model = null!;
    private MalomViewModel _viewModel = null!;

    #endregion

    #region Application methods

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        BindingPlugins.DataValidators.RemoveAt(0);

        // Modell inicializálása
        _model = new GameModel(new GamePersistence());
        _model.MalomFormed += Model_MalomFormed;
        _model.GameOver += Model_GameOver;
        _model.InvalidAction += Model_InvalidAction;
        _model.NewGame();

        // ViewModel inicializálása
        _viewModel = new MalomViewModel(_model);
        _viewModel.NewGame += ViewModel_NewGame;
        _viewModel.ExitGame += ViewModel_ExitGame;
        _viewModel.LoadGame += ViewModel_LoadGame;
        _viewModel.SaveGame += ViewModel_SaveGame;

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = _viewModel
            };

            // Kilépés megerősítése
            desktop.MainWindow.Closing += async (s, e) =>
            {
                e.Cancel = true;
                var result = await MessageBoxManager.GetMessageBoxStandard(
                    "Malom",
                    "Biztos, hogy ki akar lépni a játékból?",
                    ButtonEnum.YesNo,
                    Icon.Question).ShowAsync();

                if (result == ButtonResult.Yes)
                {
                    desktop.Shutdown();
                }
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    #endregion

    #region ViewModel event handlers

    /// <summary>
    /// Új játék eseménykezelője.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ViewModel_NewGame(object? sender, EventArgs e)
    {
        _model.NewGame();
        _viewModel.RefreshStatusText();
    }

    /// <summary>
    /// Kilépés eseménykezelője.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ViewModel_ExitGame(object? sender, EventArgs e)
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow?.Close();
        }
    }

    /// <summary>
    /// Játék betöltés eseménykezelője.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void ViewModel_LoadGame(object? sender, EventArgs e)
    {
        if (ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop || desktop.MainWindow?.StorageProvider is null)
        {
            return;
        }

        try
        {
            var files = await desktop.MainWindow.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Játék betöltése",
                AllowMultiple = false,
                FileTypeFilter = new[] 
                { 
                    new FilePickerFileType("Malom állás") 
                    { 
                        Patterns = new[] { "*.txt" } 
                    } 
                }
            });

            if (files.Count > 0)
            {
                // A path lekérése StorageProvider-en keresztül
                var path = files[0].Path.LocalPath;
                _model.LoadGame(path);

                await MessageBoxManager.GetMessageBoxStandard("Betöltés", "Játék sikeresen betöltve!", ButtonEnum.Ok, Icon.Info).ShowAsync();
            }
        }
        catch (GameDataException)
        {
            await MessageBoxManager.GetMessageBoxStandard(
                    "Hiba",
                    "A fájl betöltése sikertelen!", 
                    ButtonEnum.Ok, Icon.Error)
                .ShowAsync();
        }
    }


    /// <summary>
    /// Játék mentés eseménykezelője.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void ViewModel_SaveGame(object? sender, EventArgs e)
    {
        if (ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop || desktop.MainWindow?.StorageProvider is null)
        {
            return;
        }

        try
        {
            var file = await desktop.MainWindow.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = "Játék mentése",
                FileTypeChoices = new[] 
                { 
                    new FilePickerFileType("Malom tábla") 
                    { 
                        Patterns = new[] { "*.txt" } 
                    } 
                }
            });

            if (file != null)
            {
                var path = file.Path.LocalPath;
                _model.SaveGame(path);
                await MessageBoxManager.GetMessageBoxStandard("Mentés", "Játék sikeresen mentve!", ButtonEnum.Ok, Icon.Info).ShowAsync();
            }
        }
        catch (Exception ex)
        {
            await MessageBoxManager.GetMessageBoxStandard(
                    "Hiba",
                    "A fájl mentése sikertelen!" + ex.Message,
                    ButtonEnum.Ok, Icon.Error)
                .ShowAsync();
        }
    }

    #endregion

    #region Model event handlers

    /// <summary>
    /// Malom alkotás eseménykezelője.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Model_MalomFormed(object? sender, MalomFormedEventArgs e)
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            MessageBoxManager.GetMessageBoxStandard(
                "Malmot alkotott!",
                $"{e.Player} malmot alkotott!\n\nKattintson egy ellenfél bábujára a levételhez.",
                ButtonEnum.Ok,
                Icon.Info).ShowAsync();
        });
    }

    /// <summary>
    /// Játék vége eseménykezelője.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Model_GameOver(object? sender, GameOverEventArgs e)
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

    /// <summary>
    /// Érvénytelen lépés eseménykezelője.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Model_InvalidAction(object? sender, InvalidActionEventArgs e)
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            MessageBoxManager.GetMessageBoxStandard(
                "Hiba",
                $"Érvénytelen lépés!\n\n{e.Message}",
                ButtonEnum.Ok,
                Icon.Warning).ShowAsync();
        });
    }

    #endregion
}