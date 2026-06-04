using CommunityToolkit.Mvvm.Input;
using SameGame.Models;
using System;
using System.Collections.ObjectModel;

namespace SameGame.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly GameModel _model;

        // Parancs az új játékhoz
        public RelayCommand NewGameCommand { get; set; }

        // Új parancs a nehézség váltáshoz (string paramétert vár)
        public RelayCommand<string> ChangeModeCommand { get; set; }

        public ObservableCollection<FieldVM> Fields { get; set; }
        public event EventHandler? BoardChanged;
        public event EventHandler? NewGame;

        public MainWindowViewModel(GameModel model)
        {
            _model = model;
            // Ha a modellben változik a tábla (pl. törlés vagy új játék miatt), frissítjük a nézetet
            _model.BoardChanged += (s, e) => UpdateView();

            Fields = new ObservableCollection<FieldVM>();
            for (int row = 0; row < 15; row++)
            {
                for (int col = 0; col < 15; col++)
                {
                    var field = new FieldVM(_model, row, col);
                    field.ClickCommand = new RelayCommand(() => OnFieldClicked(field));
                    Fields.Add(field);
                }
            }

            NewGameCommand = new RelayCommand(OnNewGame);

            ChangeModeCommand = new RelayCommand<string>(OnChangeMode);

            _model.NewGame();
        }

        private void OnFieldClicked(FieldVM field)
        {
            _model.deletColor(field.Row, field.Col);
            // Az UpdateView hívása automatikus a BoardChanged esemény miatt
        }

        // Ez fut le, ha valamelyik nehézségi gombra kattintanak
        private void OnChangeMode(string? modeStr)
        {
            // Megpróbáljuk a szöveget (pl. "Hard") átalakítani GameMode típussá
            if (Enum.TryParse(modeStr, out GameMode mode))
            {
                _model.ChangeMode(mode);
            }
        }

        public void UpdateView()
        {
            foreach (var field in Fields)
            {
                field.Refresh();
            }
        }

        private void OnNewGame()
        {
            _model.NewGame();
        }
    }
}
