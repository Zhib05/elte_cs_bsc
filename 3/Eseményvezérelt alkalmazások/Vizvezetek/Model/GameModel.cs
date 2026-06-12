using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vizvezetek.Model
{
    // Csatípusok
    public enum PipeType { Straight, Curve }

    // EGYETLEN MEZŐ LOGIKÁJA
    public class PipeItem
    {
        public PipeType Type { get; private set; }
        public int Rotation { get; private set; } // 0..3

        // Esemény, ha változik az állapot (feliratkozáshoz)
        public event EventHandler? StateChanged;

        public PipeItem(PipeType type, int rotation)
        {
            Type = type;
            Rotation = rotation;
        }

        // Üzleti logika: a forgatás szabálya
        public void Rotate()
        {
            Rotation++;
            // Jelezzük a feliratkozóknak (ViewModel), hogy változás történt
            if (StateChanged != null)
            {
                StateChanged(this, EventArgs.Empty);
            }
        }
    }

    // A JÁTÉK FŐ LOGIKÁJA (Tábla generálás, méretezés)
    public class GameModel
    {
        public int Size { get; private set; }
        public PipeItem[,] Board { get; private set; }
        private Random _random;

        // Esemény, ha új játék kezdődik (a ViewModelnek tudnia kell róla)
        public event EventHandler? GameStarted;

        public GameModel()
        {
            _random = new Random();
            Size = 12; // Alapértelmezett

            // JAVÍTÁS: Inicializáljuk a Board-ot, hogy ne legyen null warning
            // Egy ideiglenes üres táblával vagy azonnal indítunk egy játékot
            Board = new PipeItem[Size, Size];
            StartNewGame();
        }

        public void SetSize(int newSize)
        {
            Size = newSize;
            StartNewGame();
        }

        public void StartNewGame()
        {
            Board = new PipeItem[Size, Size];

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    // 0 vagy 1 véletlenszerűen
                    PipeType type = (_random.Next(0, 2) == 0) ? PipeType.Straight : PipeType.Curve;
                    // 0..3 forgatás
                    int rot = _random.Next(0, 4);
                    Board[i, j] = new PipeItem(type, rot);
                }
            }

            // Értesítjük a ViewModelt
            GameStarted?.Invoke(this, EventArgs.Empty);
        }
    }
}
