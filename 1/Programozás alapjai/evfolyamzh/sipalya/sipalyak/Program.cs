using System;
namespace sipalyak
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int napdb, palyadb, a, b, cdb, i;
            bool van1, van2;
            int[,] vastag;
            int[] cy, dy;

            beolvasas(out napdb, out palyadb, out vastag, out i);

            a1(out a, out van1, vastag, palyadb);
            b1(out b, out van2, napdb, vastag, i);
            c(out cdb, out cy, napdb, palyadb, vastag, i);
            d(out dy, palyadb, napdb, vastag, i);
            kiir(a, b, cdb, cy, dy);
        }

        static void kiir(int a, int b, int cdb, int[] cy, int[] dy)
        {
            Console.WriteLine("#");
            Console.WriteLine(a);
            Console.WriteLine("#");
            Console.WriteLine(b);
            Console.WriteLine("#");
            Console.Write(cdb);
            Console.Write(' ');
            for (int i = 1; i <= cdb; ++i)
            {
                Console.Write("{0} ", cy[i]);
            }
            Console.WriteLine();
            Console.WriteLine("#");
            for (int i = 1; i < dy.Length; ++i)
            {
                Console.Write(dy[i] + " ");
            }
        }

        static void d(out int[] dy, int palyadb, int napdb, int[,] vastag, int i)
        {
            dy = new int[palyadb + 1];
            for (i = 1; i <= palyadb; ++i)
            {
                dy[i - 1 + 1] = count(i, napdb, vastag);
            }
        }

        static int count(int i, int napdb, int[,] vastag)
        {
            int db = 0;
            for (int j = 2; j <= napdb; ++j)
            {
                if (vastag[j - 1, i] < vastag[j, i])
                {
                    db++;
                }
            }
            return db;
        }

        static void c(out int cdb, out int[] cy, int napdb, int palyadb, int[,] vastag, int i)
        {
            cdb = 0;
            cy = new int[napdb + 1];
            for (i = 1; i <= napdb; ++i)
            {
                if (eldont(i, palyadb, vastag))
                {
                    cdb++;
                    cy[cdb] = i;
                }
            }
        }

        static bool eldont(int i, int palyadb, int[,] vastag)
        {
            int j = 1;
            while (j <= palyadb && vastag[i, j] != 0)
            {
                ++j;
            }
            return j > palyadb;
        }
        static void b1(out int b, out bool van2, int napdb, int[,] vastag, int i)
        {
            van2 = false;
            int maxert = 0;
            b = -1;
            for (i = 1; i <= napdb; ++i)
            {
                if (van2 && vastag[i,1] < 50)
                {
                    if (maxert < vastag[i, 1])
                    {
                        b = i;
                    }
                } else if (!van2 && vastag[i,1] < 50)
                {
                    van2 = true;
                    maxert = vastag[i, 1];
                    b = i;
                }
            }
        }
        static void a1(out int a, out bool van1, int[,] vastag, int palyadb)
        {
            a = 1;
            while (a <= palyadb && !(vastag[1,a] > 50))
            {
                ++a;
            }
            van1 = a <= palyadb;

            if (!van1)
            {
                a = -1;
            }
        }

        static void beolvasas(out int napdb, out int palyadb, out int[,] vastag, out int i)
        {
            string[] sor;
            sor = Console.ReadLine().Split(' ');
            napdb = int.Parse(sor[0]);
            palyadb = int.Parse(sor[1]);

            vastag = new int[napdb + 1, palyadb + 1];
            for (i = 1; i <= napdb; ++i)
            {
                sor = Console.ReadLine().Split(' ');
                for (int j = 1; j <= palyadb; ++j)
                {
                    vastag[i, j] = int.Parse(sor[j - 1]);
                }
            }
        }
    }
}
