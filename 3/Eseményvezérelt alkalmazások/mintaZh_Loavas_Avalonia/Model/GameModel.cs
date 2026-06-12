using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mintaZh_Loavas_Avalonia.Model
{
    public class GameModel
    {
        public class PlayerTurnChangedEventArgs : EventArgs
        {
            private readonly Player _currentPlayer;

            public PlayerTurnChangedEventArgs(Player p)
            {
                _currentPlayer = p;
            }

            public Player CurrentPlayer => _currentPlayer;
        }

        public class GameOverEventArgs : EventArgs
        {
            private readonly Player _winner;
            public GameOverEventArgs(Player p)
            {
                _winner = p;
            }
            public Player Winner => _winner;
        }

        public enum Player { None, Player1, Player2 }

        private Player _currentPlayer = Player.Player1;
        private Player[,] _board;
        public Tuple<int, int> CurrentPlayer1Position { get; set; }
        public Tuple<int, int> CurrentPlayer2Position { get; set; }
        public Player CurrentPlayer
        {
            get => _currentPlayer;
            set
            {
                if (_currentPlayer != value)
                {
                    _currentPlayer = value;
                }
            }
        }

        public Player[,] Board => _board;

        public event EventHandler<PlayerTurnChangedEventArgs>? PlayerTurnChanged;
        public event EventHandler<GameOverEventArgs>? GameOver;

        public GameModel()
        {
            _board = new Player[8, 8];
            _board[_board.GetLength(0) - 1, _board.GetLength(1) - 1] = Player.Player1;
            _board[0, 0] = Player.Player2;
            CurrentPlayer1Position = Tuple.Create(_board.GetLength(0) - 1, _board.GetLength(1) - 1);
            CurrentPlayer2Position = Tuple.Create(0, 0);
        }

        public void NewGame()
        {
            _board = new Player[8, 8];
            _board[_board.GetLength(0) - 1, _board.GetLength(1) - 1] = Player.Player1;
            _board[0, 0] = Player.Player2;
            CurrentPlayer = Player.Player1;
        }

        public void MakeMove(int fromRow, int fromCol, int toRow, int toCol)
        {
            if (IsValidMove(fromRow, fromCol, toRow, toCol))
            {
                _board[toRow, toCol] = _board[fromRow, fromCol];

                if (CurrentPlayer == Player.Player1)
                {
                    CurrentPlayer1Position = Tuple.Create(toRow, toCol);
                }
                else
                {
                    CurrentPlayer2Position = Tuple.Create(toRow, toCol);
                }

                CurrentPlayer = CurrentPlayer == Player.Player1 ? Player.Player2 : Player.Player1;
                PlayerTurnChanged?.Invoke(this, new PlayerTurnChangedEventArgs(CurrentPlayer));

                IsGameOver(CurrentPlayer);
            }
        }

        public void IsGameOver(Player player)
        {
            bool hasValidMove = false;
            // A huszár 8 lehetséges ugrása
            int[] dx = { -2, -2, -1, -1, 1, 1, 2, 2 };
            int[] dy = { -1, 1, -2, 2, -2, 2, -1, 1 };
            int currentRow, currentCol;

            // Megnézzük, kinek a pozícióját kell vizsgálni
            if (player == Player.Player1)
            {
                currentRow = CurrentPlayer1Position.Item1;
                currentCol = CurrentPlayer1Position.Item2;
            }
            else
            {
                currentRow = CurrentPlayer2Position.Item1;
                currentCol = CurrentPlayer2Position.Item2;
            }

            // Végigpróbáljuk a lehetséges lépéseket
            for (int i = 0; i < 8; i++) // Ha átírod L-alakr, itt i < 8 lesz
            {
                int newRow = currentRow + dx[i];
                int newCol = currentCol + dy[i];

                if (IsValidMove(currentRow, currentCol, newRow, newCol))
                {
                    hasValidMove = true;
                    break; // Találtunk lépést, nem kell tovább keresni
                }
            }

            // HIBA JAVÍTVA: Akkor van vége, ha NINCS érvényes lépés
            if (!hasValidMove)
            {
                // Ha a "player" nem tud lépni, akkor a MÁSIK játékos nyert 
                Player winner = (player == Player.Player1) ? Player.Player2 : Player.Player1;
                GameOver?.Invoke(this, new GameOverEventArgs(winner));
            }
        }

        private bool IsValidMove(int fromRow, int fromCol, int toRow, int toCol)
        {
            if (toRow < 0 || toRow >= _board.GetLength(0) || toCol < 0 || toCol >= _board.GetLength(1))
                return false;
            if (_board[toRow, toCol] != Player.None)
                return false;
            int rowDiff = Math.Abs(toRow - fromRow);
            int colDiff = Math.Abs(toCol - fromCol);
            // L-alak: egyik irányba 2, másikba 1 lépés
            return (rowDiff == 2 && colDiff == 1) || (rowDiff == 1 && colDiff == 2);
        }
    }
}
