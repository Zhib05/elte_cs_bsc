using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Malom.Model;

namespace Malom.Persistence
{
    public class GamePersistence : IGamePersistence
    {
        /// <summary>
        /// Játék mentése.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="board"></param>
        /// <param name="currentPlayer"></param>
        /// <param name="currentPhase"></param>
        /// <param name="p1PiecesLeft"></param>
        /// <param name="p2PiecesLeft"></param>
        /// <param name="p1OnBoard"></param>
        /// <param name="p2OnBoard"></param>
        /// <param name="isRemoving"></param>
        /// <exception cref="GameDataException"></exception>
        public void SaveGame(string filePath, int[] board, int currentPlayer, int currentPhase, int p1PiecesLeft, int p2PiecesLeft, int p1OnBoard, int p2OnBoard, bool isRemoving)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    // Első sor: CurrentPlayer, CurrentPhase, IsRemoving
                    writer.WriteLine($"{currentPlayer} {currentPhase} {(isRemoving ? 1 : 0)}");

                    // Második sor: Számlálók
                    writer.WriteLine($"{p1PiecesLeft} {p2PiecesLeft} {p1OnBoard} {p2OnBoard}");

                    // Harmadik sor: Board tömb (24 érték szóközökkel)
                    string boardLine = string.Join(" ", board);
                    writer.WriteLine(boardLine);
                }
            }
            catch (Exception ex)
            {
                throw new GameDataException("Hiba a játékállapot mentése közben: " + ex.Message);
            }
        }

        /// <summary>
        /// Játék betöltése.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <exception cref="GameDataException"></exception>
        public (int[] board, int currentPlayer, int currentPhase, int p1PiecesLeft, int p2PiecesLeft, int p1OnBoard, int p2OnBoard, bool isRemoving) LoadGame(string filePath)
        {
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    // Első sor olvasása: CurrentPlayer, CurrentPhase, IsRemoving
                    string? line = reader.ReadLine();
                    if (string.IsNullOrEmpty(line)) throw new GameDataException("Üres fájl vagy hiányzó sor.");
                    string[] parts = line.Split(' ');
                    if (parts.Length != 3) throw new GameDataException("Hibás formátum az első sorban.");

                    int playerInt = int.Parse(parts[0]);
                    int phaseInt = int.Parse(parts[1]);
                    bool isRemoving = int.Parse(parts[2]) == 1;

                    // Második sor: Számlálók
                    line = reader.ReadLine();
                    if (string.IsNullOrEmpty(line)) throw new GameDataException("Hiányzó második sor.");
                    parts = line.Split(' ');
                    if (parts.Length != 4) throw new GameDataException("Hibás formátum a második sorban.");

                    int p1Left = int.Parse(parts[0]);
                    int p2Left = int.Parse(parts[1]);
                    int p1OnBoard = int.Parse(parts[2]);
                    int p2OnBoard = int.Parse(parts[3]);

                    // Harmadik sor: Board tömb
                    line = reader.ReadLine();
                    if (string.IsNullOrEmpty(line)) throw new GameDataException("Hiányzó harmadik sor.");
                    parts = line.Split(' ');
                    if (parts.Length != 24) throw new GameDataException("A Board tömb nem 24 elemből áll.");

                    int[] board = new int[24];
                    for (int i = 0; i < 24; i++)
                    {
                        board[i] = int.Parse(parts[i]);
                    }

                    return (board, playerInt, phaseInt, p1Left, p2Left, p1OnBoard, p2OnBoard, isRemoving);
                }
            }
            catch (Exception ex)
            {
                throw new GameDataException("Hiba a játékállapot betöltése közben: " + ex.Message);
            }
        }
    }
}
