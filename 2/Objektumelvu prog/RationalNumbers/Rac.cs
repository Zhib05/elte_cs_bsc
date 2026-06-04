using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RationalNumbers
{
    internal class Rac
    {
        class MyException : Exception { }

        private int _n, _d;

        public Rac(int i, int j)
        {
            if (j == 0)
            {
                throw new MyException();
            }
            _n = i;
            _d = j;
        }

        public static Rac operator +(Rac a, Rac b)
        {
            return new Rac(a._n * b._d + a._d * b._n, a._d * b._d);
        }

        public static Rac operator -(Rac a, Rac b)
        {
            return new Rac(a._n * b._d - a._d *b._n, a._d * b._d);
        }
        public static Rac operator *(Rac a, Rac b)
        {
            return new Rac(a._n * b._n, a._d * b._d);
        }
        public static Rac operator /(Rac a, Rac b)
        {
            if (b._n == 0)
            {
                throw new MyException();
            }
            return new Rac(a._n * b._d, a._d * b._n);
        }

        public override string ToString()
        {
            return $"Rac({_n},{_d})";
        }
    }
}
