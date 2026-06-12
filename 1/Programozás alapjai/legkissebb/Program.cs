using System;
namespace legkissebb
{
    internal class Program
    {
        struct Homerseklet
        {
            public int min;
            public int max;
        }
        static void Main(string[] args)
        {
            int n, i, sorszam;

            n = int.Parse(Console.ReadLine());

            Homerseklet[] x = new Homerseklet[n];
            for (i = 1; i <= n; ++i)
            {
                x[i - 1].min = int.Parse(Console.ReadLine());
                x[i - 1].max = int.Parse(Console.ReadLine());
            }

            sorszam = 1;
            for (i = 2; i < n; ++i)
            {
                if ((x[i - 1].max - x[i - 1].min) < (x[sorszam - 1].max - x[sorszam - 1].min))
                {
                    sorszam = i;
                }
            }
            Console.WriteLine(sorszam);
        }
    }
}
