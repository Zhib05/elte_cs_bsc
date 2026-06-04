using System;
namespace leghidegebbnap
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int nap, minh,minind, i;
            int[] h;

            int.TryParse(Console.ReadLine(), out nap);

            h = new int[nap];
            for (i = 1; i <= nap; ++i)
            {
                int.TryParse(Console.ReadLine(), out h[i - 1]);
            }

            minh = h[1]; minind = 1;
            for (i = 1 ; i <= nap ; ++i)
            {
                if (h[i - 1] < minh)
                {
                    minh = h[i - 1];
                    minind = i;
                }
            }
            Console.WriteLine("{0}", minh);
        }
    }
}
