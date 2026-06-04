using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Malom.Model;

namespace Malom.Avalonia.ViewModels
{
    /// <summary>
    /// Malom játék mezőjének típusa.
    /// </summary>
    public class FieldVM : ViewModelBase
    {
        #region Fields

        private readonly GameModel _model;

        #endregion

        #region Properties

        /// <summary>
        /// Mező indexe a táblán.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Mező háttérszíne.
        /// </summary>
        public string Background => _model.Board[Index] switch
        {
            Player.Player1 => "Red",
            Player.Player2 => "Blue",
            _ => "White"
        };

        /// <summary>
        /// Mező kattintás parancsa.
        /// </summary>
        public RelayCommand ClickCommand { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Konstruktor.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="index"></param>
        public FieldVM(GameModel model, int index)
        {
            _model = model;
            Index = index;
            ClickCommand = new RelayCommand(Click);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Mező kattintás kezelője.
        /// </summary>
        private void Click()
        {
            if (_model.IsRemoving)
                _model.RemoveOpponentPiece(Index);
            else
                _model.MakeMove(Index);
        }

        /// <summary>
        /// Mező frissítése.
        /// </summary>
        public void Refresh()
        {
            OnPropertyChanged(nameof(Background));
        }

        #endregion
    }
}
