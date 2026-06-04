using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attack.Model
{
    public class FigureChangedEventArgs : EventArgs
    {
        public required FigureData Data { get; init; }
        public required Position NewPosition { get; init; }
        public required Position OldPosition { get; init; }
    }

    public class GameModel
    {
        private List<Figure> _figures;

        public int Size { get; init; }

        public event EventHandler<FigureChangedEventArgs>? FigureChanged;
        public event EventHandler<Player>? GameWon;

        public FigureData? GetByPosition(int row, int col)
        {
            // Megkeressük a listában, van-e bábu az adott pozíción.
            return _figures.FirstOrDefault(f => f.Position.Row == row && f.Position.Col == col)?.Data;
        }

        // Az éppen soron következő bábu mindig a lista legelső eleme (0. index).
        public FigureData CurrentFigure => _figures[0].Data;

        public GameModel(int size)
        {
            Size = size;
            _figures = new List<Figure>();

            // --- BÁBUK FELVÉTELE A LISTÁBA ---
            // A hozzáadás sorrendje határozza meg, hogy ki mikor lép!
            // 1. bábu (Piros 1-es)
            _figures.Add(new Figure
            {
                Data = new FigureData { Owner = Player.Red, Id = 1 },
                Position = new Position { Row = Size - 2, Col = 0 }
            });
            // 2. bábu (Kék 1-es)
            _figures.Add(new Figure
            {
                Data = new FigureData { Owner = Player.Blue, Id = 1 },
                Position = new Position { Row = 1, Col = Size - 1 }
            });
            // ... a többi bábu ...
            _figures.Add(new Figure
            {
                Data = new FigureData { Owner = Player.Red, Id = 2 },
                Position = new Position { Row = Size - 2, Col = 1 }
            });
            _figures.Add(new Figure
            {
                Data = new FigureData { Owner = Player.Blue, Id = 2 },
                Position = new Position { Row = 1, Col = Size - 2 }
            });
            _figures.Add(new Figure
            {
                Data = new FigureData { Owner = Player.Red, Id = 3 },
                Position = new Position { Row = Size - 1, Col = 1 }
            });
            _figures.Add(new Figure
            {
                Data = new FigureData { Owner = Player.Blue, Id = 3 },
                Position = new Position { Row = 0, Col = Size - 2 }
            });
            _figures.Add(new Figure
            {
                Data = new FigureData { Owner = Player.Red, Id = 4 },
                Position = new Position { Row = Size - 1, Col = 0 }
            });
            _figures.Add(new Figure
            {
                Data = new FigureData { Owner = Player.Blue, Id = 4 },
                Position = new Position { Row = 0, Col = Size - 1 }
            });
        }

        public void Move(int row, int col)
        {
            // 1. Pályán kívüli kattintás ellenőrzése
            if (row < 0 || row >= Size || col < 0 || col >= Size)
                return;

            // Mivel mindig a lista elején lévő bábu jön, lekérjük a 0. elemet.
            var currentFigure = _figures[0];

            // Kiszámoljuk, mennyit mozdult a bábu
            var diffRow = row - currentFigure.Position.Row;
            var diffCol = col - currentFigure.Position.Col;

            // 2. Szabálytalan lépés szűrése:
            // Túl messze lépett (>1) VAGY helyben maradt (0).
            if (Math.Abs(diffRow) > 1 || Math.Abs(diffCol) > 1 || Math.Abs(diffRow) + Math.Abs(diffCol) == 0)
                return;

            // 3. Van-e ott saját bábu? (Arra nem léphetünk)
            if (_figures.Any(f => f.Position.Row == row && f.Position.Col == col &&
            f.Data.Owner == currentFigure.Data.Owner))
                return;

            // Megnézzük, van-e ott ellenfél (ütés lehetősége)
            var collisionFigure = _figures.FirstOrDefault(f => f.Position.Row == row && f.Position.Col == col);
            if (collisionFigure != null)
            {
                // Ha saját, akkor return (bár ezt fentebb már szűrtük)
                if (collisionFigure.Data.Owner == currentFigure.Data.Owner)
                    return;

                // ÜTÉS SZABÁLY: Csak átlósan szabad ütni!
                // Ha nem átlós (vagyis nem mindkét koordináta változott 1-gyel), akkor érvénytelen.
                if (Math.Abs(diffRow) != 1 || Math.Abs(diffCol) != 1)
                    return;
            }

            // --- LÉPÉS VÉGREHAJTÁSA ---
            var oldPosition = currentFigure.Position;
            currentFigure.Position = new Position { Row = row, Col = col };

            // Ha volt ott ellenfél, azt töröljük a listából (LEÜTÉS)
            if (collisionFigure != null)
            {
                _figures.Remove(collisionFigure);
            }

            // --- FORGATÁS (ROTATION) ---
            // A lépés végén az aktuális bábu a sor végére kerül.
            // 1. Kivesszük az elejéről (ő volt a 0.)
            _figures.RemoveAt(0);
            // 2. Betesszük a lista végére
            _figures.Add(currentFigure);

            // Jelezzük a felületnek (ViewModel), hogy változás történt
            FigureChanged?.Invoke(this, new FigureChangedEventArgs
            {
                Data = currentFigure.Data,
                NewPosition = currentFigure.Position,
                OldPosition = oldPosition
            });

            // --- GYŐZELEM ELLENŐRZÉSE ---
            // Piros célja: (0, Size-1) sarok
            if (currentFigure.Data.Owner == Player.Red &&
                currentFigure.Position.Row == 0 && currentFigure.Position.Col == Size - 1)
            {
                GameWon?.Invoke(this, Player.Red);
            }
            // Kék célja: (Size-1, 0) sarok
            else if (currentFigure.Data.Owner == Player.Blue &&
                currentFigure.Position.Row == Size - 1 && currentFigure.Position.Col == 0)
            {
                GameWon?.Invoke(this, Player.Blue);
            }
        }
    }
}
