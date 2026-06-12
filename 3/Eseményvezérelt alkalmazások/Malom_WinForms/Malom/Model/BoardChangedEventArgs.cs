using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malom.Model
{
    /// <summary>
    /// Eseményargumentum osztály a tábla változásának jelzésére.
    /// </summary>
    public class BoardChangedEventArgs : EventArgs
    {
        private readonly int _position;
        private readonly Player _player;

        public BoardChangedEventArgs(int pos, Player p)
        {
            _position = pos;
            _player = p;
        }

        /// <summary>
        /// Pozíció, ahol a változás történt.
        /// </summary>
        public int Position => _position;

        /// <summary>
        /// Játékos, aki a változást okozta.
        /// </summary>
        public Player Player => _player;
    }
}
