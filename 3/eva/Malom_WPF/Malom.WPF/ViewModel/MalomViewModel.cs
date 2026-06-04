using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Malom.Model;

namespace Malom.ViewModel
{
    /// <summary>
    /// Malom játék nézetmodellje.
    /// </summary>
    public class MalomViewModel : ViewModelBase
    {
        /// <summary>
        /// Játék modell.
        /// </summary>
        private readonly GameModel _model;

        /// <summary>
        /// Új játék parancsa.
        /// </summary>
        public DelegateCommand NewGameCommand { get; private set; }

        /// <summary>
        /// Játék betöltés parancsa.
        /// </summary>
        public DelegateCommand LoadGameCommand { get; private set; }
        
        /// <summary>
        /// Játék mentés parancsa.
        /// </summary>
        public DelegateCommand SaveGameCommand { get; private set; }
        
        /// <summary>
        /// Kilépés parancsa.
        /// </summary>
        public DelegateCommand ExitCommand { get; private set; }

        /// <summary>
        /// Új játék eseménye.
        /// </summary>
        public event EventHandler? NewGame;

        /// <summary>
        /// Játék betöltés eseménye.
        /// </summary>
        public event EventHandler? LoadGame;

        /// <summary>
        /// Játék mentés eseménye.
        /// </summary>
        public event EventHandler? SaveGame;

        /// <summary>
        /// Kilépés eseménye.
        /// </summary>
        public event EventHandler? ExitGame;

        /// <summary>
        /// Játék státusz szövege.
        /// </summary>
        public string StatusText { get; private set; } = "Player 1 kezd (Lerakás fázis)";

        /// <summary>
        /// Mezők gyűjteménye.
        /// </summary>
        public ObservableCollection<FieldVM> Fields { get; }

        /// <summary>
        /// Konstruktor.
        /// </summary>
        /// <param name="model"></param>
        public MalomViewModel(GameModel model)
        {
            _model = model;

            Fields = new ObservableCollection<FieldVM>();
            for (int i = 0; i < 24; i++)
                Fields.Add(new FieldVM(_model, i));

            _model.PlayerTurnChanged += (s, e) =>
            {
                StatusText = $"{e.CurrentPlayer} következik | Fázis: {_model.CurrentPhase}";
                if (_model.IsRemoving) 
                    StatusText += " (Bábu levétele)";
                OnPropertyChanged(nameof(StatusText));
            };

            _model.BoardChanged += (s, e) => Fields[e.Position].Refresh();

            NewGameCommand = new DelegateCommand(param => OnNewGame());
            LoadGameCommand = new DelegateCommand(param => OnLoadGame());
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());
            ExitCommand = new DelegateCommand(param => OnExitGame());
        }

        /// <summary>
        /// Új játék eseménykiváltója.
        /// </summary>
        private void OnNewGame()
        {
            NewGame?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Játék betöltés eseménykiváltója.
        /// </summary>
        private void OnLoadGame()
        {
            LoadGame?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Játék mentés eseménykiváltója.
        /// </summary>
        private void OnSaveGame()
        {
            SaveGame?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Kilépés eseménykiváltója.
        /// </summary>
        private void OnExitGame()
        {
            ExitGame?.Invoke(this, EventArgs.Empty);
        }
    }
}