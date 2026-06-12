using System;
namespace gyak1
{
    internal class Program
    {
        static int Main(string[] args)
        {
            string filePath;
            do
            {
                Console.WriteLine("File path: ");
                filePath = Console.ReadLine();
            } while (filePath == null || !File.Exists(filePath) || Path.GetExtension(filePath) != ".txt");

            IDocumentStatistics documentStatistics = new DocumentStatistics(filePath);

            try
            {
                documentStatistics.Load();
                Console.WriteLine(documentStatistics.FileContent);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                return -1;
            }

            //var pairs = documentStatistics.DistinctWordCount.OrderByDescending(p => p.Value);


            return 0;
        }
    }
}
