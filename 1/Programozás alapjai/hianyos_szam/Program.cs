namespace hianyos_szam
{
    internal class Program
    {
        static void hianyose(int a)
        {
            int b = 1;
            for (int i = 1; i < a; i++)
            {
                if (a % i == 0)
                {
                    b += i;
                }
            }
            if (b < a)
            {
                Console.WriteLine("{0} egy hianyos szam", a);
            }
            else
            {
                Console.WriteLine("{0} nem hianyos szam", a);
            }
        }
        static void Main(string[] args)
        {
            int n;
            n = int.Parse(Console.ReadLine());
            hianyose(n);
        }
    }
}
