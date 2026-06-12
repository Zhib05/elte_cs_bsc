// Wang Zhibo(CRGYRX) wangzhibo08@gmail.com
using System;
namespace _2bead
{
    internal class Program
    {
        struct Reszletek
        {
            public int le;
            public int fel;
        }
        static void Main(string[] args)
        {
            int allomas, ar, vonatk, i, tbevetel, gazd;

            allomas = int.Parse(Console.ReadLine());
            string[] input = Console.ReadLine().Split(' ');
            ar = int.Parse(input[0]);
            vonatk = int.Parse(input[1]);

            Reszletek[] allomasadat = new Reszletek [allomas + 1];
            for (i = 1; i <= allomas; ++i)
            {
                string[] rekord = Console.ReadLine().Split(' ');
                allomasadat[i].le = int.Parse(rekord[0]);
                allomasadat[i].fel = int.Parse(rekord[1]);
            }

            tbevetel = 0;
            for (i = 1; i <= allomas; ++i)
            {
                tbevetel += aktualisutas(i, allomasadat) * ar;
            }

            if  (tbevetel > allomas * vonatk)
                gazd = 1;
            else
                gazd = 0;

            Console.WriteLine(gazd);
        }

        static int aktualisutas (int adottmegallo, Reszletek[] allomasadat)
        {
            int utasok, i;
            utasok = 0;
            for (i = 1; i <= adottmegallo; ++i)
            {
                utasok = utasok + allomasadat[i].fel - allomasadat[i].le;
            }
            return utasok;
        }
    }
}
