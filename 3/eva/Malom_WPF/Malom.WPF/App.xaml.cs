using System.ComponentModel;
using System.Windows;
using Malom.Model;
using Malom.Persistence;
using Malom.ViewModel;
using Microsoft.Win32;

namespace Malom.WPF;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private GameModel _model = null!;
    private MalomViewModel _viewModel = null!;
    private MainWindow _view = null!;

    /// <summary>
    /// Konstruktor.
    /// </summary>
    public App()
    {
        Startup += App_Startup;
    }

    /// <summary>
    /// Alkalmazás indítása.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void App_Startup(object? sender, StartupEventArgs e)
    {
        _model = new GameModel(new GamePersistence());
        _model.MalomFormed += Model_MalomFormed;
        _model.GameOver += Model_GameOver;
        _model.InvalidAction += Model_InvalidAction;
        _model.NewGame();

        _viewModel = new MalomViewModel(_model);
        _viewModel.NewGame += ViewModel_NewGame;
        _viewModel.ExitGame += ViewModel_ExitGame;
        _viewModel.LoadGame += ViewModel_LoadGame;
        _viewModel.SaveGame += ViewModel_SaveGame;

        _view = new MainWindow();
        _view.DataContext = _viewModel;
        _view.Closing += View_Closing;
        _view.Show();
    }

    /// <summary>
    /// Ablak bezárás kezelője.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void View_Closing(object? sender, CancelEventArgs e)
    {
        if (MessageBox.Show("Biztos, hogy ki akar lépni a játékból?",
            "Malom", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
        {
            e.Cancel = true;
        }
    }

    /// <summary>
    /// Új játék kezelője.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ViewModel_NewGame(object? sender, EventArgs e)
    {
        _model.NewGame();
    }

    /// <summary>
    /// Játék betöltés kezelője.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ViewModel_LoadGame(object? sender, EventArgs e)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog
        {
            Filter = "Malom állás|*.txt",
            Title = "Játék betöltése"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            try
            {
                _model.LoadGame(openFileDialog.FileName);
                MessageBox.Show("Játék sikeresen betöltve!", "Betöltés",
                               MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba a betöltés során:\n" + ex.Message, "Hiba",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    /// <summary>
    /// Játék mentés kezelője.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ViewModel_SaveGame(object? sender, EventArgs e)
    {
        SaveFileDialog saveFileDialog = new SaveFileDialog
        {
            Filter = "Malom állás|*.txt",
            FileName = "malom.txt",
            Title = "Játék mentése"
        };

        if (saveFileDialog.ShowDialog() == true)
        {
            try
            {
                _model.SaveGame(saveFileDialog.FileName);
                MessageBox.Show("Játék sikeresen mentve!", "Mentés",
                               MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba a mentés során:\n" + ex.Message, "Hiba",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    /// <summary>
    /// Kilépés kezelője.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ViewModel_ExitGame(object? sender, EventArgs e)
    {
        _view.Close();
    }

    /// <summary>
    /// Malom alkotás kezelője.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Model_MalomFormed(object? sender, MalomFormedEventArgs e)
    {
        MessageBox.Show($"{e.Player} malmot alkotott!\n\nKattintson egy ellenfél bábujára a levételhez.",
                       "Malmot alkotott!", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    /// <summary>
    /// Játék vége kezelője.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Model_GameOver(object? sender, GameOverEventArgs e)
    {
        MessageBox.Show($"Játék vége!\n\nGYŐZTES: {e.Winner}\n\nGratulálunk!",
                       "Játék vége", MessageBoxButton.OK, MessageBoxImage.Asterisk);
    }

    /// <summary>
    /// Érvénytelen lépés kezelője.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Model_InvalidAction(object? sender, InvalidActionEventArgs e)
    {
        MessageBox.Show($"Érvénytelen lépés!\n\n{e.Message}", "Hiba",
                       MessageBoxButton.OK, MessageBoxImage.Warning);
    }
}

