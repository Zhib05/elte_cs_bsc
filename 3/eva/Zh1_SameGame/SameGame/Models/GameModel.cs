using System;
using System.Collections.Generic;


namespace SameGame.Models
{
    public enum GameMode
    {
        Easy,
        Medium,
        Hard
    }

    public enum Color
    {
        Red,
        Green,
        Blue,
        Orange,
        Pink,
        Yellow,
        Gray,
        None
    }

    public class GameModel
    {
        private Random _random = new Random();
        private Color[,] _board = new Color[15, 15];
        public GameMode Mode { get; set; } = GameMode.Easy;

        public Color[,] Board => _board;

        public event EventHandler? BoardChanged;

        public GameModel()
        {
            // A konstruktorban csak indítunk egy új játékot
            NewGame();
        }

        // Segédfüggvény, hogy ne kelljen mindig direktben a tömböt elérni
        public Color GetColor(int row, int col)
        {
            if (row < 0 || row >= 15 || col < 0 || col >= 15)
                return Color.None;
            return _board[row, col];
        }

        public void deletColor(int row, int col)
        {
            // 1. Ellenőrzés: Pályán belül vagyunk-e?
            if (row < 0 || row >= 15 || col < 0 || col >= 15) return;

            // 2. Ellenőrzés: Ha már üres mezőre kattintottunk, ne történjen semmi
            if (_board[row, col] == Color.None) return;

            Color targetColor = _board[row, col];

            // Itt tároljuk, hogy mely mezőket kell törölni
            List<(int, int)> itemsToDelete = new List<(int, int)>();

            // Segédtömb, hogy tudjuk, melyik mezőt néztük már meg (hogy ne kerüljünk végtelen ciklusba)
            bool[,] visited = new bool[15, 15];

            // Megkeressük az összes szomszédot
            SearchNeighbors(row, col, targetColor, itemsToDelete, visited);

            // Ha találtunk elemeket (önmagán kívül is, vagy csak önmagát), akkor töröljük őket
            // A szabály szerint általában klikk-re törlődik, akkor is ha egyedül van.
            if (itemsToDelete.Count > 0)
            {
                foreach (var item in itemsToDelete)
                {
                    _board[item.Item1, item.Item2] = Color.None;
                }

                // Miután töröltünk, jöhet a "leesés"
                ApplyGravity();

                // Jelezzük a felületnek, hogy frissüljön
                BoardChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        // Ez a rekurzív függvény keresi meg a szomszédokat
        private void SearchNeighbors(int r, int c, Color target, List<(int, int)> items, bool[,] visited)
        {
            // Pálya szélének ellenőrzése
            if (r < 0 || r >= 15 || c < 0 || c >= 15) return;

            // Ha már láttuk ezt a mezőt, nem foglalkozunk vele
            if (visited[r, c]) return;

            // Ha nem olyan a színe, mint amit keresünk, megállunk
            if (_board[r, c] != target) return;

            // Megjelöljük, hogy itt jártunk, és hozzáadjuk a törlendők listájához
            visited[r, c] = true;
            items.Add((r, c));

            // Megnézzük a négy irányt (Fel, Le, Balra, Jobbra)
            // Itt hívja meg önmagát a függvény (rekurzió)
            SearchNeighbors(r - 1, c, target, items, visited); // Fel
            SearchNeighbors(r + 1, c, target, items, visited); // Le
            SearchNeighbors(r, c - 1, target, items, visited); // Balra
            SearchNeighbors(r, c + 1, target, items, visited); // Jobbra
        }

        // Ez intézi, hogy a kockák leessenek a lyukak helyére
        private void ApplyGravity()
        {
            // Oszloponként megyünk végig (balról jobbra)
            for (int col = 0; col < 15; col++)
            {
                // Egy oszlopon belül lentről felfelé haladunk
                // 'writeRow' mutatja, hova kell leesnie a következő kockának
                int writeRow = 14;

                for (int row = 14; row >= 0; row--)
                {
                    if (_board[row, col] != Color.None)
                    {
                        // Ha találunk egy kockát, azt "leírjuk" a writeRow pozícióba
                        _board[writeRow, col] = _board[row, col];

                        // Ha a writeRow nem egyezik a row-val (tehát esett a kocka), 
                        // akkor az eredeti helyét töröljük (bár a következő körben úgyis felülíródhat, de a tetején fontos)
                        if (writeRow != row)
                        {
                            _board[row, col] = Color.None;
                        }

                        // A következő kocka eggyel feljebb fog érkezni
                        writeRow--;
                    }
                }

                // A maradék helyeket a tetején feltöltjük üressel (biztonság kedvéért)
                while (writeRow >= 0)
                {
                    _board[writeRow, col] = Color.None;
                    writeRow--;
                }
            }
        }

        public void NewGame()
        {
            _board = new Color[15, 15];

            // Meghatározzuk hány szín legyen a nehézség alapján
            int colorCount = 3; // Easy alapértelmezett
            if (Mode == GameMode.Medium) colorCount = 5;
            if (Mode == GameMode.Hard) colorCount = 7;

            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    // Véletlen szám generálása a színek számának megfelelően
                    int rand = _random.Next(colorCount);

                    // Egyszerű hozzárendelés
                    if (rand == 0) _board[i, j] = Color.Red;
                    else if (rand == 1) _board[i, j] = Color.Green;
                    else if (rand == 2) _board[i, j] = Color.Blue;
                    else if (rand == 3) _board[i, j] = Color.Orange;
                    else if (rand == 4) _board[i, j] = Color.Pink;
                    else if (rand == 5) _board[i, j] = Color.Yellow;
                    else if (rand == 6) _board[i, j] = Color.Gray;
                }
            }

            // Értesítjük a nézetet, hogy új pálya van
            BoardChanged?.Invoke(this, EventArgs.Empty);
        }

        public void ChangeMode(GameMode mode)
        {
            Mode = mode;
            NewGame();
        }
    }
}
