/* Készítette: Wang Zhibo
   Neptun: CRGYRX
   Email: crgyrx@inf.elte.hu
   Feladat: Leghűvösebb települések legmelegebb napjai*/
using System;
using System.Security.Cryptography;
namespace crgyrx
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // deklarálás
            int legnhom, db;
            int[] y, minnapert;
            int[,] hom;

            // beolvasás
            hom = beolvas();

            // feldolgozás
            feldolgozas(out legnhom, out minnapert, out db, out y, hom);

            // kiírás
            kiiras(db, y);
        }

        static int[,] beolvas()
        {
            if (Console.IsInputRedirected)
            {
                return beolvas_biro();
            } else
            {
                return beolvas_kezi();
            }
        }

        static int[,] beolvas_biro()
        {
            string[] sor;
            sor = Console.ReadLine().Split(' ');
            int telep = int.Parse(sor[0]);
            int elorejelzes = int.Parse(sor[1]);

            int[,] hom = new int[telep, elorejelzes];
            for (int i = 0; i < telep; ++i)
            {
                sor = Console.ReadLine().Split(' ');
                for (int j = 0; j < elorejelzes; ++j)
                {
                    hom[i, j] = int.Parse(sor[j]);
                }
            }

            return hom;
        }

        static int[,] beolvas_kezi()
        {
            int telep, elorejelzes;
            bool jo;
            do
            {
                Console.ResetColor();
                Console.Write("Települések szama = ");
                jo = int.TryParse(Console.ReadLine(), out telep) && telep >= 1;
                if (!jo)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("0-tól nagyobb természetes szám kell!");
                }
            } while (!jo);
            do
            {
                Console.ResetColor();
                Console.Write("Napok szama = ");
                jo = int.TryParse(Console.ReadLine(), out elorejelzes) && elorejelzes >= 1;
                if (!jo)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("0-tól nagyobb természetes szám kell!");
                }
            } while (!jo);

            int[,] hom = new int[telep, elorejelzes];
            for (int i = 0; i < telep; ++i)
            {
                for (int j = 0; j < elorejelzes; ++j)
                {
                    do
                    {
                        Console.ResetColor();
                        Console.Write("{0}. település {1}. napon előrejelzet hőmérséklete = ", i + 1, j + 1);
                        jo = int.TryParse(Console.ReadLine(), out hom[i, j]) && hom[i, j] >= -50 && hom[i, j] <= 50;
                        if (!jo)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("-50 és 50 közé eső egész szám kell!");
                        }
                    } while (!jo);
                }
            }

            return hom;
        }

        static int minnap(int i, int[,] hom, int telep)
        {
            int minert = hom[0, i];
            int j;
            for (j = 1; j < telep; ++j)
            {
                if (minert > hom[j, i])
                {
                    minert = hom[j, i];
                }
            }
            return minert;
        }

       static void feldolgozas(out int legnhom, out int[] minnapert, out int db, out int[] y, int[,] hom)
        {
            int i, telep, elorejelzes;
            telep = hom.GetLength(0);
            elorejelzes = hom.GetLength(1);

            y = new int[elorejelzes];
            minnapert = new int[elorejelzes];

            for (i = 0; i < elorejelzes; ++i)
            {
                minnapert[i] = minnap(i, hom, telep);
            }

            legnhom = minnapert[0];
            for (i = 1; i < elorejelzes; ++i)
            {
                if (legnhom < minnapert[i])
                {
                    legnhom = minnapert[i];
                }
            }

            db = 0;
            for (i = 0; i < elorejelzes; ++i)
            {
                if (minnap(i, hom, telep) == legnhom)
                {
                    ++db;
                    y[db - 1] = i + 1;
                }
            }
        }

        static void kiiras(int db, int[] y)
        {
            if (Console.IsInputRedirected || Console.IsOutputRedirected)
            {
                Console.Write(db);
                for (int i = 0; i < db; ++i)
                {
                    Console.Write(" {0}", y[i]);
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                if (db == 0)
                {
                    Console.WriteLine("Nincs a feltételnek megfelelő nap!");
                } else
                {
                    Console.WriteLine("{0} darab feltételnek megfelelő nap van, melyek ezek a napok:", db);
                    for (int i = 0; i < db - 1; ++i)
                    {
                        Console.Write("{0}, ", y[i]);
                    }
                    Console.WriteLine(y[db - 1]);
                }
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.Write("Kérem, nyomjon ENTER-t a folytatáshoz!");
                Console.ResetColor();
                Console.ReadLine();
            }
        }
    }
}
