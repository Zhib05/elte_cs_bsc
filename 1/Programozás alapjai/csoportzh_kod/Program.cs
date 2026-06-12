using System;
using System.Threading.Tasks.Dataflow;
namespace csoportzh_kod
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int n, i, ix, j, jx, k, q, iw;
            bool jo;
            n = 8;
            string[] m = new string[n];
            int[] v = new int[n];

            for (ix = 0; ix < n; ++ix)
            {
                m[ix] = "";
                for (jx = 0; jx < n; ++jx)
                {
                    m[ix] = m[ix] + ".";
                }
                v[ix] = -1;
            }

            i = 0;
            while (i < n)
            {
                j = v[i] + 1;
                k = 0;
                while (k < i && !(v[k] == j || Math.Abs(v[k] - j) == i - k))
                {
                    ++k;
                }

                jo = k >= i;
                while (j < n && !jo)
                {
                    ++j;
                    k = 0;
                    while (k < i && !(v[k] == j || (Math.Abs(v[k] - j) == i - k)))
                    {
                        ++k;
                    }
                    jo = k >= i;
                }
                if ( j < n)
                {
                    v[i] = j;
                    m[i] = "";
                    for (q = 0; q < n; ++q)
                    {
                        if (q == v[i])
                        {
                            m[i] = m[i] + 'X';
                        }
                        else
                        {
                            m[i] = m[i] + '.';
                        }
                        ++i;
                    }
                }
                else
                {
                    m[i] = "";
                    for (q = 0; q < n; ++q)
                    {
                        m[i] = m[i] + '.';
                    }
                    v[i] = -1;
                    --i;
                }

                for (iw = 0; iw < n; ++iw)
                {
                    Console.WriteLine("{0} ", m[iw]);
                }
                Console.WriteLine();
            }
        }
    }
}
