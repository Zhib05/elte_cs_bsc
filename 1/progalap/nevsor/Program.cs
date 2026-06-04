using System;
using System;
namespace nevsor
{
    internal class Program
    {
        struct Nevmag
        {
            public string nev;
            public int magassag;
        }
        static void Main(string[] args)
        {
            int n, i;
            bool ih;

            n = int.Parse(Console.ReadLine());

            Nevmag[] p = new Nevmag[n];
            for (i = 1; i <= n; i++)
            {
                Console.Write("az {0} ember neve: ", i);
                p[i - 1].nev = Console.ReadLine();

                Console.Write("az {0} ember mag: ", i);
                p[i - 1].magassag = int.Parse(Console.ReadLine());

                Console.WriteLine("{0} {1}", p[i - 1].nev, p[i - 1].magassag);
            }

            i = 1;
            while (i <= n && !(p[i - 1].magassag <= p[i+1-1].magassag))
            {
                i = i + 1;
            }
            ih = i <= n;

            Console.WriteLine("{0}", ih);
        }
    }
}
