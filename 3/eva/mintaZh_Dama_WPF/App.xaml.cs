using System.Windows;
using mintaZh_Dama_WPF.Model;
using mintaZh_Dama_WPF.ViewModel;

namespace Dama_WPF
{
    public partial class App : Application
    {
        public App()
        {
            this.Startup += App_Startup;
        }

        private void App_Startup(object? sender, StartupEventArgs e)
        {
            // 1. Model létrehozása
            DamaModel _model = new DamaModel();

            // 2. Feliratkozás a Model eseményeire közvetlenül
            _model.GameOver += Model_GameOver;
            _model.MoveError += Model_MoveError;

            // 3. ViewModel létrehozása (átadjuk neki a Modelt)
            DamaViewModel _viewModel = new DamaViewModel(_model);

            // 4. View létrehozása
            MainWindow _view = new MainWindow();
            _view.DataContext = _viewModel;
            _view.Show();
        }

        private void Model_MoveError(object? sender, string message)
        {
            MessageBox.Show(message, "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void Model_GameOver(object? sender, string message)
        {
            MessageBox.Show(message, "Játék vége", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}