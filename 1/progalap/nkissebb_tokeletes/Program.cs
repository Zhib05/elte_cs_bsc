using System;
namespace nkissebb_tokeletes
{
    internal class Program
    {
        static void tokeletese(int a)
        {
            int b = 0;
            for (int i = 1; i < a; i++)
            {
                for (int j = 1; j < i; j++)
                {
                    if (i % j == 0)
                    {
                        b += j;
                    }
                }
                if (b == i)
                {
                    Console.WriteLine("{0}", i);
                }
                b = 0;
            }
        }
        static void Main(string[] args)
        {
            int n;
            n = int.Parse(Console.ReadLine());
            tokeletese(n);
        }
    }
}
