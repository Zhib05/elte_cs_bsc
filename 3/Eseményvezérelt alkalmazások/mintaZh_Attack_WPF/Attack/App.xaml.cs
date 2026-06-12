using Attack.Model;
using Attack.ViewModel;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Attack
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Startup += App_Startup;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            GameModel model = new GameModel(6);
            model.GameWon += Model_GameWon;

            GameViewModel vm = new GameViewModel(model);
            MainWindow window = new MainWindow
            {
                DataContext = vm
            };
            window.Show();
        }

        private void Model_GameWon(object? sender, Player e)
        {
            MessageBox.Show($"{e} player has won!", "Game Over", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

}
