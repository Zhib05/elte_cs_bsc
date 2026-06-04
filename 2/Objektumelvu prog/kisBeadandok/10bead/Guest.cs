using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HF10
{
    public class Guest
    {
        private string _name;
        private List<Gift> _prizes;

        public Guest(string n)
        {
            _name = n;
            _prizes = new List<Gift>();
        }

        public string getName() => _name;

        public void Wins(Gift g)
        {
            if (!g.targetShot.GetGifts().Contains(g))
            {
                throw new Exception("Gift not in target shot.");
            }
            g.targetShot.GetGifts().Remove(g);
            _prizes.Add(g);
        }

        public int Result(TargetShot t)
        {
            int total = 0;
            foreach (Gift e in _prizes)
            {
                if (e.targetShot == t)
                {
                    total += e.Value();
                }
            }

            return total;
        }
    }
}
