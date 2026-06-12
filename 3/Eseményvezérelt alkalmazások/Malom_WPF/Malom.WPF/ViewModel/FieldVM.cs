using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Malom.Model;
using Malom.ViewModel;

namespace Malom.ViewModel
{
    /// <summary>
    /// Malom játék mezőjének típusa.
    /// </summary>
    public class FieldVM : ViewModelBase
    {
        /// <summary>
        /// Játék modell.
        /// </summary>
        private readonly GameModel _model;

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
        public DelegateCommand ClickCommand { get; }

        /// <summary>
        /// Konstruktor.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="index"></param>
        public FieldVM(GameModel model, int index)
        {
            _model = model;
            Index = index;
            ClickCommand = new DelegateCommand(param => Click());
        }

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
    }
}
