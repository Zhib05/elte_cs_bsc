using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mintaZh_BlockDocu_WinForms.Model
{
    public class GameModel
    {
        // A 4x4-es játéktábla. true = foglalt (kék), false = üres (fehér).
        private bool[,] _board = new bool[4, 4];
        private Random _random = new Random();

        // Alakzat definíció: a bool tömb a relatív blokk pozíciókat adja meg.
        public class Shape
        {
            public bool[,] Layout { get; }

            public Shape(bool[,] layout)
            {
                Layout = layout;
            }
        }

        //[cite_start]// A lehetséges alakzatok listája (két 2-blokkos, két 3-blokkos L-alakú) [cite: 5, 6]
        private readonly List<Shape> _availableShapes = new List<Shape>
        {
            // 2-blokkos vízszintes
            new Shape(new bool[,] { {false, false }, { true, true} }), 
            // 2-blokkos függőleges
            new Shape(new bool[,] { { true }, { true } }), 
            // 3-blokkos L-alak (2x2 területen belül)
            new Shape(new bool[,] { { true, false }, { true, true } }), 
            // 3-blokkos tükrözött L-alak (2x2 területen belül)
            new Shape(new bool[,] { { true, true }, { false, true } })
        };

        public Shape? CurrentShape { get; private set; }

        // Események a Nézet értesítésére
        public event EventHandler? GameBoardChanged;
        public event EventHandler? NewShapeAvailable;

        public GameModel()
        {
            //[cite_start]// Kezdéskor generálunk egy alakzatot 
            GenerateNextShape();
        }

        // 1. és 2. részfeladat: Új alakzat generálása
        public void GenerateNextShape()
        {
            int index = _random.Next(_availableShapes.Count);
            CurrentShape = _availableShapes[index];
            // Értesíti a nézetet, hogy mutassa meg az új alakzatot
            NewShapeAvailable?.Invoke(this, EventArgs.Empty);
        }

        // 1. és 2. részfeladat: Ellenőrzés és elhelyezés
        public bool TryPlaceShape(int startRow, int startCol)
        {
            if (CurrentShape == null) return false;

            int shapeHeight = CurrentShape.Layout.GetLength(0);
            int shapeWidth = CurrentShape.Layout.GetLength(1);

            //[cite_start]// 1. Ellenőrzés: Kilóg-e a tábláról? [cite: 8, 14, 15]
            if (startRow + shapeHeight > 4 || startCol + shapeWidth > 4)
            {
                return false;
            }

            //[cite_start]// 2. Ellenőrzés: Fed-e le már foglalt (kék) blokkot? [cite: 8, 21]
            for (int i = 0; i < shapeHeight; i++)
            {
                for (int j = 0; j < shapeWidth; j++)
                {
                    // Ha az alakzat blokkja 'true' ÉS a táblán is foglalt (true), akkor ütközés van
                    if (CurrentShape.Layout[i, j] && _board[startRow + i, startCol + j])
                    {
                        return false; // Lefed egy kék blokkot
                    }
                }
            }

            //[cite_start]// 3. Sikeres elhelyezés: Frissíti a táblát (kékre színezi a blokkokat) [cite: 15]
            for (int i = 0; i < shapeHeight; i++)
            {
                for (int j = 0; j < shapeWidth; j++)
                {
                    if (CurrentShape.Layout[i, j])
                    {
                        _board[startRow + i, startCol + j] = true;
                    }
                }
            }

            // Sorok/oszlopok ellenőrzése és tisztítása (2. részfeladat)
            CheckAndClearRowsAndColumns();

            // Értesíti a nézetet a tábla változásáról
            GameBoardChanged?.Invoke(this, EventArgs.Empty);

            //[cite_start]// Csak sikeres elhelyezés után generálódik a következő alakzat [cite: 22]
            GenerateNextShape();

            return true;
        }

        //[cite_start]// 2. részfeladat: Sorok és oszlopok ellenőrzése és tisztítása [cite: 9, 22]
        private void CheckAndClearRowsAndColumns()
        {
            // Sorok ellenőrzése
            for (int r = 0; r < 4; r++)
            {
                bool rowFilled = true;
                for (int c = 0; c < 4; c++)
                {
                    if (!_board[r, c])
                    {
                        rowFilled = false;
                        break;
                    }
                }
                if (rowFilled)
                {
                    for (int c = 0; c < 4; c++)
                    {
                        _board[r, c] = false;
                    }
                }
            }

            // Oszlopok ellenőrzése
            for (int c = 0; c < 4; c++)
            {
                bool colFilled = true;
                for (int r = 0; r < 4; r++)
                {
                    if (!_board[r, c])
                    {
                        colFilled = false;
                        break;
                    }
                }
                if (colFilled)
                {
                    for (int r = 0; r < 4; r++)
                    {
                        _board[r, c] = false;
                    }
                }
            }
        }

        // Segédmetódus a nézetnek a tábla állapotának lekérdezéséhez
        public bool GetCellStatus(int row, int col)
        {
            return _board[row, col];
        }
    }
}
