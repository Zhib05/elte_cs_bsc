using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Idoutazo.Model
{
    public struct Positioin
    {
        public int Row {  get; set; }
        public int Col { get; set; }
    }
    public class Piece
    {
        public Player player;
        public double Hp { get; set; } = 100;
        public int Row { get; set; }
        public int Col { get; set; }

        public Piece(Player player,  float hp, int row, int col)
        {
            this.player = player;
            this.Hp = hp;
        }
    }

    public enum Player
    {
        Player1,
        Player2,
        None
    }
    public class GameModel
    {
        public Piece[,] Board { get; set; }
        public Player CurrentPlayer { get; set; } = Player.Player1;

        public event EventHandler? BoardChanged;
        public GameModel()
        {
            Board = new Piece[7, 7];
            NewGame();
        }

        public void NewGame()
        {
            for (int i = 0;  i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if (j == 0 && i % 2 != 0)
                    {
                        Board[i, j] = new Piece(Player.Player1, 100, i, j);
                    }
                    else if (j == 6 && i % 2 != 0)
                    {
                        Board[i, j] = new Piece(Player.Player2, 100, i, j);
                    }
                    else
                    {
                        Board[i, j] = new Piece(Player.None, 100, i, j);
                    }
                }
            }
            BoardChanged?.Invoke(this, EventArgs.Empty);
        }

        public Player GetPlayer(int row, int col)
        {
            return Board[row, col].player;
        }

        public void Step(int fromX, int fromY, int toX, int toY)
        {
            if (!IsValidMove(fromX, fromY, toX, toY)) return;

            if (Board[toX, toY].player == Player.None)
            {
                Board[toX, toY] = Board[fromX, fromY];
                Board[fromX, fromY] = new Piece(Player.None, 100, fromX, fromY);
            } else if (Board[toX, toX].player == Board[fromX, fromY].player)
            {
                var temp = Board[toX, toY];
                Board[toX, toY] = Board[fromX, fromY];
                Board[fromX, fromY] = temp;
            } else if (Board[toX, toY].player != Board[fromX, fromY].player)
            {
                Board[toX, toY].Hp -= Board[fromX, fromY].Hp * 0.4;
                if (Board[toX, toY].Hp <= 0)
                {
                    Board[toX, toY] = new Piece(Player.None, 100, toX, toY);
                } else
                {
                    Board[fromX, fromY].Hp -= Board[toX, toY].Hp * 0.4;
                }
            }

            CurrentPlayer = CurrentPlayer == Player.Player1 ? Player.Player2 : Player.Player1;

            BoardChanged?.Invoke(this, EventArgs.Empty);
        }

        private bool IsValidMove(int fromX, int fromY, int toX, int toY)
        {
            if (fromX < 0 || fromX >= 7 || fromY < 0 || fromY >= 7) return false;
            if (toX < 0 || toX >= 7 || toY < 0 || toY >= 7) return false;

            if (GetPlayer(fromX, fromY) != CurrentPlayer) return false;

            int dx = fromX - toX;
            int dy = fromY - toY;

            if (Math.Abs(dx) == 1 && Math.Abs(dy) == 0) return true;
            else if (Math.Abs(dx) == 0 && Math.Abs(dy) == 1) return true;
            else if (Math.Abs(dx) == 1 && Math.Abs(dy) == 1) return true;

            return false;
        }
    }
}
