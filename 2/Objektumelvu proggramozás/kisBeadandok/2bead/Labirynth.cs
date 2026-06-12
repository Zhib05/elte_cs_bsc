using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HF1
{
    public class NoTreasureException : Exception { }
    public enum Content { EMPTY, WALL, GHOST, TREASURE };
    struct Direction
    {
        public int x;
        public int y;
    }

    internal class Labirynth
    {
        private int _n, _m;
        private Content[,] _map;

        public Labirynth(Content[,] map)
        {
            this._n = map.GetLength(0);
            this._m = map.GetLength(1);
            this._map = map;
        }

        public Content LookAt(int x, int y, Direction dir)
        {
            if (!(x + dir.x >= 0 && x + dir.x <= _n - 1 && y + dir.y >= 0 && y + dir.y <= _m - 1))
            {
                throw new IndexOutOfRangeException();
            }
            return this._map[x + dir.x, y + dir.y];
        }

        public void Collect(int x, int y)
        {
            if (this._map[x, y] != Content.TREASURE)
            {
                throw new NoTreasureException();
            }
            this._map[x, y] = Content.EMPTY;
        }
    }
}