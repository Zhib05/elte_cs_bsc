using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malom.Model
{
    /// <summary>
    /// Eseményargumentum osztály érvénytelen lépés jelzésére.
    /// </summary>
    public class InvalidActionEventArgs : EventArgs
    {
        private readonly string _message;

        public InvalidActionEventArgs(string msg)
        {
            _message = msg;
        }

        /// <summary>
        /// Hibaüzenet.
        /// </summary>
        public string Message => _message;
    }
}
