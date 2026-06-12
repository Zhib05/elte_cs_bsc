using System;
namespace paros_matrix2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int n, m, i, j, db, sordb;

            n = int.Parse(Console.ReadLine());
            m = int.Parse(Console.ReadLine());

            int[,] matrix = new int[n, m];
            for (i = 1; i <= n; ++i)
            {
                for (j = 1; j <= m; ++j)
                {
                    matrix[i,j] = int.Parse(Console.ReadLine());
                }
            }

            for (i = 1 ; i <= n ; ++i)
            {
                for(j = 1 ; j <= m ; ++j)
                {
                    Console.Write("{0} ", matrix[i - 1, j - 1]);
                    Console.WriteLine();
                }
            }

            db = 0;
            for (i = 1; i <= n; ++i)
            {
                sordb = 0;
                for (j = 1; j <= m; ++j)
                {
                    if (matrix[i - 1,j - 1] % 2 == 0)
                    {
                        ++sordb;
                    }
                    db = db + sordb;
                }
            }
            Console.WriteLine(db);
        }
    }
}
