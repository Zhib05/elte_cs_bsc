using System;
namespace karacsonyi_slagerek
{
    internal class Program
    {
        struct Adatok
        {
            public int rada;
            public int dala;
            public int dalh;
        }
        static void Main(string[] args)
        {
            int adb, rdb, ddb, dal, k, maxind, maxert, maxhossz, hanyfeldal, mindal, tobbmint180, i;
            bool van2, van4;
            int[] dalok;
            Adatok[] felmeres;

            beolvasas(out adb, out rdb, out ddb, out felmeres, out dal, out k, out i);

            a(out maxind, out maxert, out maxhossz, adb, felmeres, i);
            b(out van2, out mindal, felmeres, adb, i, dal);
            c(out hanyfeldal, out dalok, adb, felmeres, i);
            d(out tobbmint180, out van4, adb, felmeres, k);

            kiiras(maxhossz, mindal, hanyfeldal, dalok, tobbmint180, i, adb);
        }

        static void kiiras(int maxhossz, int mindal, int hanyfeledal, int[] dalok, int tobbmint180, int i, int adb)
        {
            Console.WriteLine("#");
            Console.WriteLine(maxhossz);

            Console.WriteLine("#");
            Console.WriteLine(mindal);

            Console.WriteLine("#");
            Console.Write(hanyfeledal);
            for (i = 1; i <= hanyfeledal; ++i)
            {
                Console.Write(" {0}", dalok[i]);
            }
            Console.WriteLine();

            Console.WriteLine("#");
            Console.WriteLine(tobbmint180);
        }

        static void d(out int tobbmint180,  out bool van4, int adb, Adatok[] felmeres, int k)
        {
            tobbmint180 = 1;
            while (tobbmint180 <= adb && !kdb180s(tobbmint180, felmeres, k))
            {
                ++tobbmint180;
            }
            van4 = tobbmint180 <= adb;

            if (!van4)
            {
                tobbmint180 = -1;
            }
        }

        static bool kdb180s(int tobbmint180, Adatok[] felmeres, int k)
        {
            int j = tobbmint180;
            while (j <= tobbmint180 + k - 1 && felmeres[j].dalh > 180)
            {
                ++j;
            }
            return j > tobbmint180 + k - 1;
        }

        static void c(out int hanyfeladal, out int[] dalok, int adb, Adatok[] felmeres, int i)
        {
            dalok = new int[adb + 1];
            hanyfeladal = 0;
            for (i = 1; i <= adb; ++i)
            {
                if (eldont(i, felmeres))
                {
                    hanyfeladal++;
                    dalok[hanyfeladal] = felmeres[i].dala;
                }
            }
        }

        static bool eldont(int i, Adatok[] felmeres)
        {
            int j = 1;
            while (j <= i - 1 && felmeres[i].dala != felmeres[j].dala)
            {
                ++j;
            }
            return j > i - 1;
        }

        static void b(out bool van2, out int mindal, Adatok[] felmeres, int adb, int i, int dal)
        {
            van2 = false;
            mindal = 0;
            for (i = 1; i <= adb; ++i)
            {
                if (van2 && felmeres[i].dala == dal)
                {
                    if (mindal > felmeres[i].dalh)
                    {
                        mindal = felmeres[i].dalh;
                    }
                } else if (!van2 && felmeres[i].dala == dal)
                {
                    van2 = true;
                    mindal = felmeres[i].dalh;
                }
            }

            if (!van2)
            {
                mindal = -1;
            }
        }

        static void a(out int maxind, out int maxert, out int maxhossz, int adb, Adatok[] felmeres, int i)
        {
            maxind = 1;
            maxert = felmeres[1].dalh;
            for (i = 2; i <= adb; ++i)
            {
                if (maxert < felmeres[i].dalh)
                {
                    maxind = i;
                    maxert = felmeres[i].dalh;
                }
            }
            maxhossz = felmeres[maxind].rada;
        }

        static void beolvasas(out int adb, out int rdb, out int ddb, out Adatok[] felmeres, out int dal, out int k, out int i)
        {
            string[] sor;
            sor = Console.ReadLine().Split(' ');
            adb = int.Parse(sor[0]);
            rdb = int.Parse(sor[1]);
            ddb = int.Parse(sor[2]);

            felmeres = new Adatok[adb + 1];
            for (i = 1; i <= adb; ++i)
            {
                sor = Console.ReadLine().Split(' ');
                felmeres[i].rada = int.Parse(sor[0]);
                felmeres[i].dala = int.Parse(sor[1]);
                felmeres[i].dalh = int.Parse(sor[2]);
            }

            sor = Console.ReadLine().Split(' ');
            dal = int.Parse(sor[0]);
            k = int.Parse(sor[1]);
        }
    }
}
