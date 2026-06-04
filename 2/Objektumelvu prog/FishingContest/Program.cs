using System.Globalization;

namespace FishingContest {
    public class Program {
        static void Main(string[] args) {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            try {
                Infile infile = new Infile("input1.txt");
                RunSearch(infile, out bool l, out string? name);
                if (l) {
                    Console.WriteLine($"{name} nevű horgász pontyokból legalább 10kg-ot fogott");
                } else {
                    Console.WriteLine("Nem volt olyan horgász aki legalább 10kg pontyot fogott volna.");
                }
            } catch (FileNotFoundException) {
                Console.WriteLine("Nincs ilyen file");
            }
        }

        public static void RunSearch(Infile f, out bool l, out string? name) {
            name = null;
            l = false;

            while (!l && f.Read(out Fisherman? fisherman)) {
                if (fisherman!.SumCarpWeight() >= 10) {
                    l = true;
                    name = fisherman.Name;
                }
            }
        }
    }
}
