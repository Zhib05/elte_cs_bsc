using TextFile;

namespace Algoritmusok
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RunCactusSorted("in1.txt", out List<string> y, out List<string> z);
            Console.WriteLine("[F1]> Piros kaktuszok");
            foreach (string item in y)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("[F1]> Mexikoi kaktuszok");
            foreach (string item in z)
            {
                Console.WriteLine(item);
            }

            RunSimultaneous("in2.txt", out int m, out bool l);
            Console.WriteLine($"[F2]> Legnagyobb szam: {m}, van paros szam a szamok kozt: {l}");

            RunEvenCounter("in3.txt", out int dbe, out int dbu);
            Console.WriteLine($"Párosok a negatív előtt: {dbe}, Párosok a negatív után: {dbu}");
        }

        public record struct Cactus(string Name, string Color, string Homland, int Height);
        public static void RunCactusSorted(string inFilePath, out List<string> y, out List<string> z)
        {
            y = new List<string>();
            z = new List<string>();

            static bool ReadCactus(TextFileReader reader, out Cactus cactus)
            {
                cactus = default;
                if (!reader.ReadLine(out string Line))
                {
                    return false;
                }
                string[] tokens = Line.Split();
                cactus.Name = tokens[0];
                cactus.Color = tokens[1];
                cactus.Homland = tokens[2];
                cactus.Height = int.Parse(tokens[3]);
                return true;
            }

            try
            {
                TextFileReader textReader1 = new TextFileReader(inFilePath);

                bool st1 = ReadCactus(textReader1, out Cactus e1);
                while (st1)
                {
                    if (e1.Color == "piros")
                    {
                        y.Add(e1.Name);
                    }
                    if (e1.Homland == "Mexikói")
                    {
                        z.Add(e1.Name);
                    }
                    st1 = ReadCactus(textReader1, out e1);
                }
            } catch (FileNotFoundException)
            {
                Console.WriteLine("Nem letezik ez a file");
            }

            TextFileReader textReader = new TextFileReader(inFilePath);
            bool st = ReadCactus(textReader, out Cactus e);
        }

        public static void RunSimultaneous(string inFilePath, out int m, out bool l)
        {
            m = 0;
            l = false;

            try
            {
                TextFileReader textReader = new TextFileReader(inFilePath);

                bool st = textReader.ReadInt(out int e);
                while (textReader.ReadInt(out e))
                {
                    if (e > m)
                    {
                        m = e;
                    }
                    //m = Math.Max(m, e);
                    l |= (e % 2 == 0); 
                    // l = l || (e % 2 == 0);
                }
            } catch (FileNotFoundException)
            {
                Console.WriteLine("Nem letezik ez a file");
            }
        }

        public static void RunEvenCounter(string inFilePath, out int dbe, out int dbu)
        {
            dbe = 0;
            dbu = 0;

            try
            {
                TextFileReader textReader = new TextFileReader(inFilePath);
                bool st = textReader.ReadInt(out int e);
                while (st && e >= 0)
                {
                    if (e % 2 == 0)
                    {
                        dbe += 1;
                    }
                    st = textReader.ReadInt(out e);
                }
                while (st)
                {
                    if (e % 2 == 0)
                    {
                        dbu += 1;
                    }
                    st = textReader.ReadInt(out e);
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Nem letezik ez a file");
            }
        }
    }
}
