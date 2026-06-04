using System;
using System.Data;
using System.Security.Cryptography.X509Certificates;
namespace evfolyamzh
{
    internal class Program
    {
        struct Tora
        {
            public string targynev, osztaly;
            public int oraszam, tanar;
        }
        static void Main(string[] args)
        {
            int n, t, i, maxoraszam, maxtanardb, osztalyoraidb, targyakdb;
            string oa, maxtanar;

            n = int.Parse(Console.ReadLine());
            t = int.Parse(Console.ReadLine());
            oa = Console.ReadLine();

            int[] osszoraszam = new int[t + 1];
            string[] maxtanarok = new string[n + 1];
            string[] osztalyorai = new string[n + 1];
            string[] targyak = new string[n + 1];

            string[] tanar = new string[t + 1];
            for (i = 1; i < t; ++i)
            {
                tanar[i] = Console.ReadLine();
            }

            Tora[] orak = new Tora[n + 1];
            for (i = 1; i < n; ++i)
            {
                orak[i].targynev = Console.ReadLine();
                orak[i].osztaly = Console.ReadLine();
                orak[i].tanar = int.Parse(Console.ReadLine());
                orak[i].oraszam = int.Parse(Console.ReadLine());
            }

            // a) feladat
            for (i = 1; i <= t; ++i)
            {
                osszoraszam[i] = tanaroraszam(i, orak);
                Console.WriteLine("{0} ", osszoraszam[i]);
            }

            // b) feladat
            maxoraszam = orak[1].oraszam;
            for (i = 2; i <= n; ++i)
            {
                if (orak[i].oraszam > maxoraszam)
                {
                    maxoraszam = orak[i].oraszam;
                }
            }
            maxtanardb = 0;
            for (i = 1; i <= n; ++i)
            {
                if(maxoraszam == orak[i].oraszam)
                {
                    maxtanardb++;
                    maxtanarok[maxtanardb] = tanar[orak[i].tanar];
                }
            }
            maxtanar = maxtanarok[1];
            for (i = 2; i <= maxtanardb; ++i)
            {
                if(maxtanar.CompareTo(maxtanarok[i]) > 0)
                {
                    maxtanar = maxtanarok[i];
                }
            }
            Console.WriteLine(maxtanar);

            // c) feladat
            osztalyoraidb = 0;
            for (i = 1; i <= n; ++i)
            {
                if(oa == orak[i].osztaly)
                {
                    osztalyoraidb++;
                    osztalyorai[osztalyoraidb] = orak[i].targynev;
                    Console.WriteLine(", {0}", osztalyorai[i]);
                }
            }
            Console.Write(osztalyoraidb);

            // d) feladat
            targyakdb = 0;
            for (i = 1; i <= n; ++i)
            {
                if (elso(i, orak))
                {
                    targyakdb++;
                    targyak[targyakdb] = orak[i].targynev;
                    Console.WriteLine(", {0}", targyak[i]);
                }
            }
            Console.Write(targyakdb);
        }
        static int tanaroraszam(int tanar, Tora[] orak)
        {
            int s = 0;
            for (int i = 1; i <= orak.Length-1;  ++i)
            {
                s += tanaroraja(tanar, i, orak);
            }
            return s;
        }
        static int tanaroraja(int tanar, int ora, Tora[] orak)
        {
            int ret;
            if (tanar == orak[ora].tanar)
            {
                ret = orak[ora].oraszam;
            }
            else
            {
                ret = 0;
            }
            return ret;
        }

        static bool elso(int ora, Tora[] orak)
        {
            int i = 1;
            while ((i < ora - 1) && (orak[i].targynev != orak[ora].targynev))
            {
                ++i;
            }
            return i > ora - 1;
        }
    }
}
