using Idoutazo.Model;
using Idoutazo.ViewModel;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Idoutazo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.Startup += App_Startup;
        }

        private void App_Startup(object? sender, StartupEventArgs e)
        {
            GameModel _model = new GameModel();

            GameViewModel _viewModel = new GameViewModel(_model);

            MainWindow _view = new MainWindow();
            _view.DataContext = _viewModel;
            _view.Show();
        }
    }

}
