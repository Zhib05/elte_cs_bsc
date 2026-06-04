using System;
namespace _1beadando
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int n, i, db1, db2, db3, db4, db5;
            int.TryParse(Console.ReadLine(), out n);

            int[] jegy = new int[n];
            for (i = 1; i <= n; ++i)
            {
                jegy[i - 1] = int.Parse(Console.ReadLine());
            }

            db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0;
            for (i = 1; i <= n; ++i)
            {
                if (jegy[i - 1] == 1)
                {
                    db1 = db1 + 1;
                }
                else if (jegy[i - 1] == 2)
                {
                    db2 = db2 + 1;
                }
                else if (jegy[i - 1] == 3)
                {
                    db3 = db3 + 1;
                }
                else if (jegy[i - 1] == 4)
                {
                    db4 = db4 + 1;
                }
                else if (jegy[i - 1] == 5)
                {
                    db5 = db5 + 1;
                }
            }

            Console.WriteLine("{0} {1} {2} {3} {4}", db1, db2, db3, db4, db5);
        }
    }
}
