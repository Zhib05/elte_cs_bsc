using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Malom.Model;

namespace Malom.Persistence
{
    public interface IGamePersistence
    {
        /// <summary>
        /// Játék mentése.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="board"></param>
        /// <param name="currentPlayer"></param>
        /// <param name="currentPhase"></param>
        /// <param name="p1PiecesLeft"></param>
        /// <param name="p2PiecesLeft"></param>
        /// <param name="p1OnBoard"></param>
        /// <param name="p2OnBoard"></param>
        /// <param name="isRemoving"></param>
        void SaveGame(string filename, int[] board, int currentPlayer, int currentPhase, int p1PiecesLeft, int p2PiecesLeft, int p1OnBoard, int p2OnBoard, bool isRemoving);

        /// <summary>
        /// Játék betöltése.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        (int[] board, int currentPlayer, int currentPhase, int p1PiecesLeft, int p2PiecesLeft, int p1OnBoard, int p2OnBoard, bool isRemoving) LoadGame(string filename);
    }
}
