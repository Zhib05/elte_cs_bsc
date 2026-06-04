using TextFile;
namespace HF6
{
    internal class Program
    {
        public record struct Aru(String cikkszam, int ar);
        public record struct Szamla(String nev, Aru[] lista);
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                throw new ArgumentException("Usage: Kisbead.exe <file> [<args>]");
            }

            Solution(args[0], out int ossz);
            Console.WriteLine(ossz);
        }

        public static void Solution(string inFilePath, out int bevet)
        {
            bevet = 0;

            static bool ReadSzamla(TextFileReader reader, out Szamla? sz)
            {
                sz = null;
                if (!reader.ReadLine(out string line))
                {
                    return false;
                }

                string[] tokens = line.Split(' ');

                Aru[] aruk = new Aru[(tokens.Length - 1) / 2];

                int ind = 0;
                for (int i = 1; i < tokens.Length; i += 2)
                {
                    aruk[ind] = new Aru(tokens[i], int.Parse(tokens[i + 1]));
                    ind++;
                }

                sz = new Szamla(tokens[0], aruk);
                return true;
            }

            try
            {
                TextFileReader reader = new TextFileReader(inFilePath);
                bool st = ReadSzamla(reader, out Szamla? sz);
                while (st)
                {
                    bevet += Ossz(sz.Value.lista);

                    st = ReadSzamla(reader, out sz);
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not found.");
            }
        }

        public static int Ossz(Aru[] x)
        {
            int sum = 0;
            foreach (Aru e in x)
            {
                sum += e.ar;
            }

            return sum;
        }
    }
}
