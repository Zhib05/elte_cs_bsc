using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HF10
{
    public class AmPark
    {
        private List<TargetShot> _targetShots;
        private List<Guest> _guests;

        public AmPark(List<TargetShot> t)
        {
            if (t.Count < 2)
            {
                throw new Exception("At least two target shots are required.");
            }
            _targetShots = t;
            _guests = new List<Guest>();
        }

        public string Best(TargetShot t)
        {
            if ( _guests.Count == 0)
            {
                throw new Exception("No guests have won any gifts.");
            }

            Guest elem = _guests[0];
            foreach (Guest e in _guests)
            {
                if (elem.Result(t) < e.Result(t))
                {
                    elem = e;
                }
            }
            return elem.getName();
        }

        public void Receives(Guest g)
        {
            if (_guests.Contains(g))
            {
                throw new Exception("Guest already added.");
            }
            _guests.Add(g);
        }
    }
}
