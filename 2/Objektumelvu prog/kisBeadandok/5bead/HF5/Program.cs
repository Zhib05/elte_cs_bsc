using TextFile;
namespace HF5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            double s = 0.0;
            int db = 0;

            try
            {
                TextFileReader inFilePath = new(args[0]);
                bool canRead = inFilePath.ReadDouble(out double e);
                while (canRead && e >= 0)
                {
                    s += e;
                    db++;
                    canRead = inFilePath.ReadDouble(out e);
                }

                double a = s / db;
                Console.WriteLine(a);

                bool l = true;
                double kicsi = e;
                canRead = inFilePath.ReadDouble(out e);
                while (canRead)
                {
                    l = l && e < 0;
                    if (e < kicsi)
                    {
                        kicsi = e;
                    }
                    canRead = inFilePath.ReadDouble(out e);
                }
                Console.WriteLine(l);
                Console.WriteLine(kicsi);
            } catch (FileNotFoundException)
            {
                Console.WriteLine($"file is not found: {args[0]}");
            }
        }
    }
}
