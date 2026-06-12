using System;
namespace tokeletes_szam
{
    internal class Program
    {
        static void tokeletese(int a)
        {
            int b = 0;
            for (int i = 1; i < a; i++)
            {
                if (a % i == 0)
                {
                    b += i;
                }
            }
            if (b == a)
            {
                Console.WriteLine("{0} egy tokeletes szam", a);
            }
            else
            {
                Console.WriteLine("{0} nem tokeletes szam", a);
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
