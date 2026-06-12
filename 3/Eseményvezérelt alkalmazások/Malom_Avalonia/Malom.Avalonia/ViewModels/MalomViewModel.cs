using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using Malom.Model;

namespace Malom.Avalonia.ViewModels
{
    /// <summary>
    /// Malom játék nézetmodellje.
    /// </summary>
    public class MalomViewModel : ViewModelBase
    {
        #region Fields

        private readonly GameModel _model;

        #endregion

        #region Properties

        /// <summary>
        /// Új játék parancsa.
        /// </summary>
        public RelayCommand NewGameCommand { get; private set; }

        /// <summary>
        /// Játék betöltés parancsa.
        /// </summary>
        public RelayCommand LoadGameCommand { get; private set; }
        
        /// <summary>
        /// Játék mentés parancsa.
        /// </summary>
        public RelayCommand SaveGameCommand { get; private set; }
        
        /// <summary>
        /// Kilépés parancsa.
        /// </summary>
        public RelayCommand ExitCommand { get; private set; }

        /// <summary>
        /// Játék státusz szövege.
        /// </summary>
        public string StatusText { get; private set; } = "Player 1 kezd (Lerakás fázis)";

        /// <summary>
        /// Mezők gyűjteménye.
        /// </summary>
        public ObservableCollection<FieldVM> Fields { get; }

        #endregion

        #region Events

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

        #endregion

        #region Constructor

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

            _model.PlayerTurnChanged += (s, e) => RefreshStatusText();

            _model.BoardChanged += (s, e) => Fields[e.Position].Refresh();

            NewGameCommand = new RelayCommand(OnNewGame);
            LoadGameCommand = new RelayCommand(OnLoadGame);
            SaveGameCommand = new RelayCommand(OnSaveGame);
            ExitCommand = new RelayCommand(OnExitGame);
        }

        #endregion

        #region Public method
        /// <summary>
        /// Státusz szöveg frissítése.
        /// </summary>
        public void RefreshStatusText()
        {
            StatusText = $"{_model.CurrentPlayer} következik | Fázis: {_model.CurrentPhase}";
            if (_model.IsRemoving)
                StatusText += " (Bábu levétele)";
            OnPropertyChanged(nameof(StatusText));
        }

        #endregion

        #region Private methods

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

        #endregion
    }
}