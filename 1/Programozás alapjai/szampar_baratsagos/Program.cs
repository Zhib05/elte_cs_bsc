using System;
namespace szampar_baratsagos
{
    internal class Program
    {
        static void baratsagose(int a, int b)
        {
            int c = 0, d = 0;

            for (int i = 1; i < a; i++)
            {
                if (a % i == 0)
                {
                    c += i;
                }
                if (b % i == 0)
                {
                    d += i;
                }
            }
            if (c == b && d == a)
            {
                Console.WriteLine("Az {0} es {1} baratsagos szampar", a, b);
            }
            else
            {
                Console.WriteLine("Az {0} es {1} nem baratsagos szampar", a, b);
            }
        }
        struct Szampar
        {
            public int x;
            public int y;
        }
        static void Main(string[] args)
        {
            Szampar[] par = new Szampar[1];
            par[0].x = int.Parse(Console.ReadLine());
            par[0].y = int.Parse(Console.ReadLine());
            baratsagose(par[0].x, par[0].y);
        }
    }
}
