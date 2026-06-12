using System;
namespace paros_matrix
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int n, m, i, j, db;

            n = int.Parse(Console.ReadLine());
            m = int.Parse(Console.ReadLine());

            int[,] matrix = new int [n, m];
            for (i = 1; i <= n; ++i)
            {
                Console.WriteLine("{0}. sor: ", i);
                string[] sortomb = Console.ReadLine().Split(" ");
                for (j = 1; j <= m; ++j)
                {
                    matrix[i - 1, j - 1] = int.Parse(sortomb[j - 1]);
                }
            }

            db = 0;
            for (int k = 1; k <= n * m; ++k)
            {
                if (matrix[(k - 1) / m + 1 - 1, (k - 1) % m + 1 - 1] % 2 == 0)
                {
                    ++db;
                }
            }
            Console.WriteLine(db);
        }
    }
}
