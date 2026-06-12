using System;

namespace mintaZh_Dama_WPF.Model
{
    public enum Player { White, Black }
    public enum Piece { None, White, Black }

    public class DamaModel
    {
        public Piece[,] Board { get; private set; }
        public Player CurrentPlayer { get; private set; }
        public int Size { get; } = 8;

        public event EventHandler<string>? GameOver;
        public event EventHandler<string>? MoveError;

        public DamaModel()
        {
            Board = new Piece[Size, Size];
            NewGame();
        }

        public void NewGame()
        {
            // Tábla alaphelyzetbe állítása
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Board[i, j] = Piece.None;
                    // Fekete bábuk a felső 3 sorban (0,1,2), csak a sötét mezőkön
                    if (i < 3 && (i + j) % 2 != 0) Board[i, j] = Piece.Black;
                    // Fehér bábuk az alsó 3 sorban (5,6,7)
                    if (i > 4 && (i + j) % 2 != 0) Board[i, j] = Piece.White;
                }
            }
            CurrentPlayer = Player.White;
        }

        public void Step(int fromX, int fromY, int toX, int toY)
        {
            // 1. Ellenőrizzük, hogy szabályos-e a lépés
            // Az 'out bool isCapture' megmondja, hogy történt-e ütés
            if (!IsValidMove(fromX, fromY, toX, toY, out bool isCapture))
            {
                MoveError?.Invoke(this, "Szabálytalan lépés!");
                return; // Ha hibás, kilépünk, nem történik semmi
            }

            // 2. Lépés végrehajtása: A célmezőre tesszük a bábut, a régit töröljük
            Board[toX, toY] = Board[fromX, fromY];
            Board[fromX, fromY] = Piece.None;

            // 3. Ha ütés történt, levesszük a köztes bábut
            if (isCapture)
            {
                int midX = (fromX + toX) / 2; // A két mező közötti sor
                int midY = (fromY + toY) / 2; // A két mező közötti oszlop
                Board[midX, midY] = Piece.None;
            }

            // 4. Játékos váltás (Fehér -> Fekete -> Fehér)
            CurrentPlayer = CurrentPlayer == Player.White ? Player.Black : Player.White;

            // 5. Megnézzük, vége van-e a játéknak az új állapotban
            CheckGameOver();
        }

        private bool IsValidMove(int x1, int y1, int x2, int y2, out bool isCapture)
        {
            isCapture = false;

            // Pályán kívüli kattintás szűrése
            if (x1 < 0 || x1 >= Size || y1 < 0 || y1 >= Size) return false;
            if (x2 < 0 || x2 >= Size || y2 < 0 || y2 >= Size) return false;

            // Csak a saját bábunkkal léphetünk
            Piece currentPiece = Board[x1, y1];
            if (currentPiece == Piece.None) return false;
            if (CurrentPlayer == Player.White && currentPiece != Piece.White) return false;
            if (CurrentPlayer == Player.Black && currentPiece != Piece.Black) return false;

            // A célmezőnek üresnek kell lennie
            if (Board[x2, y2] != Piece.None) return false;

            int dx = x2 - x1; // Sorok különbsége
            int dy = y2 - y1; // Oszlopok különbsége

            // Irány ellenőrzése: Fehér "felfelé" (csökkenő sorindex), Fekete "lefelé" (növekvő sorindex) léphet
            if (CurrentPlayer == Player.White && dx > 0) return false;
            if (CurrentPlayer == Player.Black && dx < 0) return false;

            // ESET A: Sima lépés (1-et lép átlósan)
            if (Math.Abs(dx) == 1 && Math.Abs(dy) == 1) return true;

            // ESET B: Ütés (2-t lép átlósan)
            if (Math.Abs(dx) == 2 && Math.Abs(dy) == 2)
            {
                int midX = (x1 + x2) / 2;
                int midY = (y1 + y2) / 2;
                Piece midPiece = Board[midX, midY];

                // Csak akkor üthetünk, ha van ott valami, és az az ellenfélé
                if (midPiece == Piece.None) return false;
                if (CurrentPlayer == Player.White && midPiece == Piece.White) return false;
                if (CurrentPlayer == Player.Black && midPiece == Piece.Black) return false;

                isCapture = true;
                return true;
            }

            return false;
        }

        private void CheckGameOver()
        {
            // Megszámoljuk, kinek hány bábuja maradt
            int currentPieces = 0;
            foreach (var p in Board)
            {
                if ((CurrentPlayer == Player.White && p == Piece.White) ||
                    (CurrentPlayer == Player.Black && p == Piece.Black))
                {
                    currentPieces++;
                }
            }

            string winner = CurrentPlayer == Player.White ? "Fekete" : "Fehér";

            // 1. feltétel: Elkfogytak a bábuk
            if (currentPieces == 0)
            {
                GameOver?.Invoke(this, $"A {winner} játékos győzött (elfogytak a bábuk)!");
                return;
            }

            // 2. feltétel: Beszorult a játékos (nincs érvényes lépés)
            if (!HasValidMoves(CurrentPlayer))
            {
                GameOver?.Invoke(this, $"A {winner} játékos győzött (nincs érvényes lépés)!");
            }
        }

        private bool HasValidMoves(Player player)
        {
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    // Ha a mi bábunkat találtuk meg...
                    Piece p = Board[x, y];
                    if ((player == Player.White && p == Piece.White) ||
                        (player == Player.Black && p == Piece.Black))
                    {
                        // ...megpróbáljuk mind a 4 irányt (sima lépés és ütés)
                        int[] offsets = { -1, 1, -2, 2 };
                        foreach (int dx in offsets)
                        {
                            foreach (int dy in new[] { -1, 1, -2, 2 })
                            {
                                if (Math.Abs(dx) != Math.Abs(dy)) continue; // Csak átlósan

                                // Ha találunk akár EGY érvényes lépést, akkor még nincs vége
                                if (IsValidMove(x, y, x + dx, y + dy, out _))
                                    return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}