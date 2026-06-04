using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackOpening
{
    public class Player
    {
        public string name;
        public Nation nationallity;

        public Player(string name, Nation nationallity)
        {
            this.name = name;
            this.nationallity = nationallity;
        }
    }
}
