using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attack.Model
{
    public struct Position
    {
        public required int Row { get; init; }
        public required int Col { get; init; }
    }

    public enum Player { Red, Blue }

    public struct FigureData
    {
        public required Player Owner { get; init; }
        public required int Id { get; init; }
    }

    public class Figure
    {
        public FigureData Data { get; init; }
        public Position Position { get; set; }
    }
}
