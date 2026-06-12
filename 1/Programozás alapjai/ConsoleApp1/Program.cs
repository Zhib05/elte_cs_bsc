using System;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // deklarálás: bemenet 
            int[,] mad;
            // deklarálás: kimenet 
            // statikus tömbbel dolgozunk, így szükség van a db-re is 
            int db;
            int[] helyseg;

            mad = beolvas();
            (db, helyseg) = kivalogat(mad);
            kiir(db, helyseg);
        }
        static int[,] beolvas()
        {
            if (Console.IsInputRedirected)
            {
                return beolvas_biro();
            }
            else
            {
                return beolvas_kezi();
            }
        }
        static int[,] beolvas_biro()
        {
            string[] sor = Console.ReadLine().Split(" ");
            int n = int.Parse(sor[0]);
            int m = int.Parse(sor[1]);

            int[,] mad = new int[n, m];
            for (int i = 0; i < n; i++)
            {
                sor = Console.ReadLine().Split(" ");
                for (int j = 0; j < m; j++)
                {
                    mad[i, j] = int.Parse(sor[j]);
                }
            }

            return mad;
        }
        static int[,] beolvas_kezi()
        {
            int n, m;
            bool jo;
            do
            {
                Console.ResetColor();
                Console.Write("Helységek száma = ");
                jo = int.TryParse(Console.ReadLine(), out n) && n >= 0;
                if (!jo)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Természetes szám kell!");
                }
            } while (!jo);
            do
            {
                Console.ResetColor();
                Console.Write("Madárfajok száma = ");
                jo = int.TryParse(Console.ReadLine(), out m) && m >= 0;
                if (!jo)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Természetes szám kell!");
                }
            } while (!jo);

            int[,] mad = new int[n, m];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    do
                    {
                        Console.ResetColor();
                        Console.Write("{0}. helyseg {1}. madárfaj darabszáma = ", i + 1, j + 1);
                        jo = int.TryParse(Console.ReadLine(), out mad[i, j]) && mad[i, j] >= 0;
                        if (!jo)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Természetes szám kell!");
                        }
                    } while (!jo);
                }
            }

            return mad;
        }
        static (int db, int[] helyseg) kivalogat(int[,] mad)
        {
            int n = mad.GetLength(0);
            int[] helyseg = new int[n];

            int db = 0;
            for (int i = 1; i <= n; i++)
            {
                if (vanmadar(i, mad) && jo(i, mad))
                {
                    db = db + 1;
                    helyseg[db - 1] = i;
                }
            }
            return (db, helyseg);
        }
        static bool vanmadar(int i, int[,] mad)
        {
            int m = mad.GetLength(1);

            int j = 1;
            while (j <= m && !(mad[i - 1, j - 1] > 0))
            {
                j = j + 1;
            }
            bool van = j <= m;
            return van;
        }
        static bool jo(int i, int[,] mad)
        {
            int m = mad.GetLength(1);

            int j = 1;
            while (j <= m && (mad[i - 1, j - 1] == 0 || masholis(i, j, mad)))
            {
                j = j + 1;
            }
            bool mind = j > m;
            return mind;
        }
        static bool masholis(int i, int j, int[,] mad)
        {
            int n = mad.GetLength(0);

            int k = 1;
            while (k <= n && !(i != k && mad[k - 1, j - 1] > 0))
            {
                k = k + 1;
            }
            bool van = k <= n;
            return van;
        }
        static void kiir(int db, int[] helyseg)
        {
            if (Console.IsOutputRedirected)
            {
                Console.WriteLine(db);
                for (int i = 0; i < db; i++)
                {
                    Console.Write("{0} ", helyseg[i]);
                }
                Console.WriteLine();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                if (db == 0)
                {
                    Console.WriteLine("Nincs a feltételnek megfelelő helység!");
                }
                else
                {
                    Console.WriteLine("{0} darab feltételnek megfelelő helység is van, sorszámaik: ", db); 
                for (int i = 0; i < db - 1; i++)
                    {
                        Console.Write("{0}, ", helyseg[i]);
                    }
                    Console.WriteLine(helyseg[db - 1]);
                }
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.WriteLine("Kérem, nyomjon ENTER-t a folytatáshoz!");
                Console.ResetColor();
                Console.ReadLine();
            }
        }
    }
}