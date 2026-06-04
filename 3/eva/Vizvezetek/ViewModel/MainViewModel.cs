using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Vizvezetek.Model;

namespace Vizvezetek.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly GameModel _gameModel; // Referencia a fő logikára

        public ObservableCollection<PipeFieldViewModel> Fields { get; set; }

        public int BoardSize
        {
            get { return _gameModel.Size; }
        }

        public DelegateCommand NewGameCommand { get; private set; }
        public DelegateCommand ChangeSizeCommand { get; private set; }

        public MainViewModel(GameModel model)
        {
            _gameModel = model;
            Fields = new ObservableCollection<PipeFieldViewModel>();

            // Feliratkozás: Ha a Model új játékot kezd, mi újraépítjük a listát
            _gameModel.GameStarted += Model_GameStarted;

            NewGameCommand = new DelegateCommand(p => _gameModel.StartNewGame());

            // Méretváltás parancs
            ChangeSizeCommand = new DelegateCommand(p =>
            {
                // JAVÍTÁS: Null ellenőrzés parsing előtt
                if (p != null && int.TryParse(p.ToString(), out int newSize))
                {
                    _gameModel.SetSize(newSize);
                }
            });

            // Inicializáláskor betöltjük a már létező táblát
            LoadTable();
        }

        private void Model_GameStarted(object? sender, EventArgs e)
        {
            LoadTable();
        }

        private void LoadTable()
        {
            Fields.Clear();
            OnPropertyChanged(nameof(BoardSize)); // Frissítjük a rács méretét a View-n

            for (int i = 0; i < _gameModel.Size; i++)
            {
                for (int j = 0; j < _gameModel.Size; j++)
                {
                    PipeItem item = _gameModel.Board[i, j];
                    Fields.Add(new PipeFieldViewModel(item));
                }
            }
        }
    }
}
