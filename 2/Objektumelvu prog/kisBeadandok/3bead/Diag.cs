using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HF3
{
    class Diag
    {
        public class InvalidSizeException : Exception { }
        public class MyException : Exception { }

        private double[] _x;

        public Diag(int n)
        {
            if (n < 0)
            {
                throw new InvalidSizeException();
            }
            _x = new double[n];
        }

        public double Get(int i, int j)
        {
            if (i < 0 || i >= _x.Length || j < 0 || j >= _x.Length)
            {
                throw new IndexOutOfRangeException();
            }
            if (i == j)
            {
                return _x[i];
            } else
            {
                return 0.0;
            }
        }

        public void Set(int i, int j, double e)
        {
            if (i < 0 || i >= _x.Length || j < 0 || j >= _x.Length)
            {
                throw new IndexOutOfRangeException();
            }
            if (i == j)
            {
                _x[i] = e;
            } else
            {
                throw new MyException();
            }
        }

        public static Diag Add(Diag a, Diag b)
        {
            if (a._x.Length != b._x.Length)
            {
                throw new MyException();
            }
            Diag c = new Diag(a._x.Length);
            for (int i = 0; i < c._x.Length; i++)
            {
                c._x[i] = a._x[i] + b._x[i];
            }
            return c;
        }

        public static Diag Multiply(Diag a, Diag b)
        {
            if (a._x.Length != b._x.Length)
            {
                throw new MyException();
            }
            Diag c = new Diag(a._x.Length);
            for (int i = 0; i < c._x.Length; i++)
            {
                c._x[i] = a._x[i] * b._x[i];
            }
            return c;
        }

        public static Diag operator +(Diag a , Diag b)
        {
            return Add(a, b);
        }

        public static Diag operator *(Diag a, Diag b)
        {
            return Multiply(a, b);
        }
    }
}
