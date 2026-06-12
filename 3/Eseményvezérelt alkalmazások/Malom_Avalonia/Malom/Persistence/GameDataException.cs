using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malom.Persistence
{
    /// <summary>
    /// Egyedi kivételosztály a játékállapot mentésével és betöltésével kapcsolatos hibák kezelésére.
    /// </summary>
    public class GameDataException : Exception
    {
        public GameDataException(string message) : base(message) { }
    }
}
