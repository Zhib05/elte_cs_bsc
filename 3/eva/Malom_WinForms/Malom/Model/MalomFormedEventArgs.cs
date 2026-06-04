using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malom.Model
{
    /// <summary>
    /// Eseményargumentum osztály malom kialakulásának jelzésére.
    /// </summary>
    public class MalomFormedEventArgs : EventArgs
    {
        private readonly Player _player;
        private readonly List<List<int>> _maloms;

        public MalomFormedEventArgs(Player p, List<List<int>> m)
        {
            _player = p;
            _maloms = m;
        }

        /// <summary>
        /// Játékos, aki malmot alakított ki.
        /// </summary>
        public Player Player => _player;

        /// <summary>
        /// Kialakult malmok pozíciói.
        /// </summary>
        public List<List<int>> Maloms => _maloms;
    }
}
