using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malom.Model
{
    /// <summary>
    /// Eseményargumentum osztály a játék végét jelző eseményhez.
    /// </summary>
    public class GameOverEventArgs : EventArgs
    {
        private readonly Player _winner;

        public GameOverEventArgs(Player w)
        {
            _winner = w;
        }

        /// <summary>
        /// Győztes játékos.
        /// </summary>
        public Player Winner => _winner;
    }
}
