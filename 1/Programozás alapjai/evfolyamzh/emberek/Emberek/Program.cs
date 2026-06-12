using System;
namespace Emberek
{
    internal class Program
    {
        struct Reszletek
        {
            public int kor;
            public int fiz;
        }
        static void Main(string[] args)
        {
            int n, i, maxert, legidosebb, negyvenfelett, hfeletkor, harmincalattdb;
            n = int.Parse(Console.ReadLine());
            int[] harmincalatt = new int[n + 1];

            Reszletek[] emberek = new Reszletek[n + 1];
            for (i = 1; i <= n; ++i)
            {
                emberek[i].kor = int.Parse(Console.ReadLine());
                emberek[i].fiz = int.Parse(Console.ReadLine());
            }

            // a) feladat
            maxert = emberek[1].kor;
            legidosebb = 1;
            for (i = 2; i <= n; ++i)
            {
                if (emberek[i].kor > maxert)
                {
                    legidosebb = i;
                }
            }
            Console.WriteLine(legidosebb);

            // b) feladat
            negyvenfelett = 0;
            for (i = 1; i <= n; ++i)
            {
                if (emberek[i].kor > 40 && emberek[i].fiz < 200000)
                {
                    ++negyvenfelett;
                }
            }
            Console.WriteLine(negyvenfelett);

            // c) feladat
            hfeletkor = 0;
            for (i = 1; i <= n; ++i)
            {
                if (fugg(i, emberek))
                {
                    ++hfeletkor;
                }
            }
            Console.WriteLine(hfeletkor);

            //d) feladat
            harmincalattdb = 0;
            for (i = 1; i <= n; ++i)
            {
                if (emberek[i].kor < 30)
                {
                    ++harmincalattdb;
                    harmincalatt[harmincalattdb] = kisebbh(i, emberek);
                    Console.Write("{0} ", harmincalatt[harmincalattdb]);
                }
            }
            Console.Write("{0}", harmincalattdb);
            Console.WriteLine("\n");
        }

        static bool fugg (int kor, Reszletek[] emberek)
        {
            bool ret; 
            int i = 1;
            while (i <= kor - 1 && emberek[i].kor != emberek[kor].kor)
            {
                ++i;
            }
            ret = i > kor - 1;
            return ret;
        }

        static int kisebbh (int a, Reszletek[] emberek)
        {
            int ind = a;
            while (!(emberek[ind].kor < 30))
            {
                ++ind;
            }
            return ind;
        }
    }
}
