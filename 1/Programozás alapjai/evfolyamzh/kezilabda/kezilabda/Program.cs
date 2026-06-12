using System;
namespace kezilabda
{
    internal class Program
    {
        struct Adatok
        {
            public string orszag;
            public int loves;
            public int gol;
            public int hat;
        }
        static void Main(string[] args)
        {
            // deklaracio
            int jdb, maxind4, maxert2, ind1, ind3;
            bool van1, van2, van3;
            string or, maxert4;
            int[] y4;

            Adatok[] jatekos;
            deklaracio(out jdb, out or, out jatekos);

            // 1) feladat
            a(out ind1, out van1, jdb, jatekos);
            // 2) feladat
            b(out van2, out maxert2, or, jdb, jatekos);
            // 3) feladat
            c(out ind3, out van3, jdb, jatekos);
            // 4) feladat
            d(out y4, out maxind4, out maxert4, jdb, jatekos);

            // kiiras
            kiir(ind1, maxert2, ind3, maxert4);
        }

        static void kiir(int ind1, int maxert2, int ind3, string maxert4)
        {
            Console.WriteLine("#");
            Console.WriteLine(ind1);
            Console.WriteLine("#");
            Console.WriteLine(maxert2);
            Console.WriteLine("#");
            Console.WriteLine(ind3);
            Console.WriteLine("#");
            Console.WriteLine(maxert4);
        }

        static void d(out int[] y4, out int maxind4, out string maxert4, int jdb, Adatok[] jatekos)
        {
            int i;
            y4 = new int[jdb + 1];
            for (i = 1; i <= jdb; i++)
            {
                y4[i] = count(i, jatekos, jdb);
            }

            maxind4 = 1;
            for (i = 2; i <= jdb; i++)
            {
                if (y4[i] > y4[1])
                {
                    maxind4 = i;
                }
            }
            maxert4 = jatekos[maxind4].orszag;
        }

        static int count(int i, Adatok[] jatekos, int jdb)
        {
            int j, db;
            db = 0;

            for (j = 1; j <= jdb; j++)
            {
                if (jatekos[j].orszag == jatekos[i].orszag)
                {
                    ++db;
                }
            }
            return db;
        }

        static void c(out int ind3, out bool van3, int jdb, Adatok[] jatekos)
        {
            van3 = false;
            ind3 = 4;
            while (ind3 <= jdb && !(eldont(ind3, jatekos)))
            {
                ind3++;
            }
            van3 = ind3 <= jdb;
            if (!van3)
            {
                ind3 = -1;
            }
        }

        static bool eldont(int ind3, Adatok[] jatekos)
        {
            int j = ind3 - 3;
            while (j <= ind3 - 1 && jatekos[j].hat < jatekos[ind3].hat)
            {
                j++;
            }
            return j > ind3 - 1;
        }

        static void b(out bool van2, out int maxert2, string or, int jdb, Adatok[] jatekos)
        {
            van2 = false;
            maxert2 = 0;
            for (int i = 1; i <= jdb; i++)
            {
                if (van2 && or == jatekos[i].orszag)
                {
                    if (jatekos[i].loves > maxert2)
                    {
                        maxert2 = jatekos[i].loves;
                    }
                }
                else if (!van2 && or == jatekos[i].orszag)
                {
                    van2 = true;
                    maxert2 = jatekos[i].loves;
                }
            }
            if (!van2)
            {
                maxert2 = -1;
            }
        }

        static void deklaracio(out int jdb, out string or, out Adatok[] jatekos)
        {
            string[] line;

            line = Console.ReadLine().Split(' ');
            jdb = int.Parse(line[0]);
            or = line[1];

            jatekos = new Adatok[jdb + 1];
            for (int i = 1; i <= jdb; i++)
            {
                line = Console.ReadLine().Split(' ');
                jatekos[i].orszag = line[0];
                jatekos[i].loves = int.Parse(line[1]);
                jatekos[i].gol = int.Parse(line[2]);
                jatekos[i].hat = int.Parse(line[3]);
            }
        }

        static void a(out int ind1, out bool van1, int jdb, Adatok[] jatekos)
        {
            ind1 = 1;
            while (ind1 <= jdb && !(jatekos[ind1].gol > 100))
                ++ind1;
            van1 = ind1 <= jdb;
            if (!van1)
            {
                ind1 = -1;
            }
        }

    }
}
