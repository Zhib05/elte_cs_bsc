using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malom.Model
{
    /// <summary>
    /// Eseményargumentum osztály a játékos körének változásának jelzésére.
    /// </summary>
    public class PlayerTurnChangedEventArgs : EventArgs
    {
        private readonly Player _currentPlayer;

        public PlayerTurnChangedEventArgs(Player p)
        {
            _currentPlayer = p;
        }

        /// <summary>
        /// Aktuális játékos.
        /// </summary>
        public Player CurrentPlayer => _currentPlayer;
    }
}
