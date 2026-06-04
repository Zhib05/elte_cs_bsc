using System;
namespace konyvtar
{
    internal class Program
    {
        struct Adatok
        {
            public int kkod;
            public int tkod;
            public int ev;
            public int db;
        }

        static void kiir(int a, int bdb, int[] by, int n)
        {
            Console.WriteLine("#");
            Console.WriteLine(a);

            Console.WriteLine("#");
            Console.WriteLine(bdb);
            for (int i = 0; i < bdb; i++)
            {
                Console.Write("{0} " ,by[i]);
            }
            Console.WriteLine();

            Console.WriteLine("#");
            Console.WriteLine("#");
            Console.WriteLine("#");
        }
        static void Main(string[] args)
        {
            int n, a, minind1, bdb;//, d1, d2, edb, c, maxind3, maxind4;
            int[] y1, by;//, y3, y4, etkod, etkoddb;
            Adatok[] konyvek;

            beolvasas(out n, out konyvek);
            a1(out a, out y1, out minind1, n, konyvek);
            b1(out bdb, out by, n, konyvek);
            kiir(a, bdb, by, n);
        }

        static bool eldont(int i, Adatok[] konyvek)
        {
            int j = 1;
            while (j <= i - 1 && konyvek[j].ev != konyvek[i].ev)
            {
                j++;
            }
            return j > i - 1;
        }

        static void b1(out int bdb, out int[] by, int n, Adatok[] konyvek)
        {
            bdb = 0;
            by = new int[n];
            for (int i = 0; i < n; ++i)
            {
                if (eldont(i, konyvek))
                {
                    bdb++;
                    by[bdb - 1] = konyvek[i].ev;
                }
            }
        }

        static int elso(int i, int j, Adatok[] konyvek)
        {
            int a = 0;
            if (konyvek[j].ev == konyvek[i].ev)
            {
                a = konyvek[j].db;
            } else
            {
                a = 0;
            }
            return a;
        }

        static int szum(int i, int n, Adatok[] konyvek)
        {
            int sum = 0;
            for (int j = 0; j < n; ++j)
            {
                sum += elso(i, j, konyvek);
            }
            return sum;
        }

        static void a1(out int a, out int[] y1, out int minind1, int n, Adatok[] konyvek)
        {
            y1 = new int[n];
            for (int i = 0; i < n; ++i)
            {
                y1[i] = szum(i, n, konyvek);
            }
            minind1 = 0;
            for (int i = 0; i < n; ++i)
            {
                if (y1[1] > y1[i])
                {
                    minind1 = i;
                }
            }
            a = konyvek[minind1].ev;
        }
        static void beolvasas(out int n, out Adatok[] konyvek)
        {
            n = int.Parse(Console.ReadLine());
            konyvek = new Adatok[n];

            string[] sor;
            for (int i = 0; i < n; ++i)
            {
                sor = Console.ReadLine().Split(' ');
                konyvek[i].kkod = int.Parse(sor[0]);
                konyvek[i].tkod = int.Parse(sor[1]);
                konyvek[i].ev = int.Parse(sor[2]);
                konyvek[i].db = int.Parse(sor[3]);
            }
        }
    }
}
