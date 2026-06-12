using System;
internal class Program
{
    private static void Main(string[] args)
    {
        int[] m;
        int a, hany, i;

        a = int.Parse(Console.ReadLine());

        m = new int[a];
        for (i = 1; i <= a; ++i)
        {
            m[i - 1] = int.Parse(Console.ReadLine());
        }

        hany = 0;
        for (i = 1; i <= a; i++)
        {
            if (m[i - 1] > 0)
            {
                hany = hany + 1;
            }
        }

        Console.WriteLine(hany);
    }
}