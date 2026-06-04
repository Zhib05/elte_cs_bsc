using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base
{
    public class InvalidDispenserException : Exception { }
    internal class Dispenser
    {
        private double _max, _dose, _act;

        public Dispenser(double a, double b)
        {
            if (!(a > 0 && b > 0))
            {
                throw new InvalidDispenserException();
            }
            _max = a;
            _dose = b;
            _act = 0.0;
        }

        public void Push()
        {
            _act = Math.Max(_act - _dose, 0.0);
        }

        public void Fill()
        {
            _act = _max;
        }

        public bool IsEmpty()
        {
            return _act == 0;
        }
    }
}
