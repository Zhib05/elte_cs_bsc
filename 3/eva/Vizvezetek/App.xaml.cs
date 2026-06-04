using System.Configuration;
using System.Data;
using System.Windows;
using Vizvezetek.Model;
using Vizvezetek.ViewModel;

namespace Vizvezetek;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // 1. Létrehozzuk a Modelt (Logika)
        GameModel gameModel = new GameModel();

        // 2. Létrehozzuk a ViewModelt, és odaadjuk neki a Modelt (Összekötés)
        MainViewModel mainViewModel = new MainViewModel(gameModel);

        // 3. Létrehozzuk a View-t (Ablak), és beállítjuk a DataContext-et
        MainWindow window = new MainWindow();
        window.DataContext = mainViewModel;

        window.Show();
    }
}

