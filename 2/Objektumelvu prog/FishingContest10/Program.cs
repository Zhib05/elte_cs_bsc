using System.IO;
using System;

namespace FishingContest10
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Organization org = ReadOrganization();

            if (org.Best(out Contest? contest))
            {
                Console.WriteLine($"A legjobb verseny helye: {contest!.Location}");
            }
            else
            {
                Console.WriteLine("Nincs olyan verseny, ahol mindenki fogott harcsát.");
            }
        }

        static Organization ReadOrganization()
        {
            Organization org = new Organization();
            char[] separators = [' ', '\t'];

            try
            {
                using StreamReader reader = new("contests.txt");

                string line = reader.ReadLine()!;
                string[] tokens = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                foreach (string name in tokens)
                {
                    org.Join(new Fisherman(name));
                }

                while (!reader.EndOfStream)
                {
                    string fileName = reader.ReadLine()!;
                    using StreamReader contestReader = new(fileName);

                    line = contestReader.ReadLine()!;
                    string[] contestDetails = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    Contest contest = org.Organize(contestDetails[0], DateTime.Parse(contestDetails[1]));

                    line = contestReader.ReadLine()!;
                    string[] fishernames = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string fishername in fishernames)
                    {
                        contest.SignUp(org.Search(fishername)!);
                    }

                    while (!contestReader.EndOfStream)
                    {
                        tokens = contestReader.ReadLine()!.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        string fishermanName = tokens[0];
                        string timeStr = tokens[1];
                        string fishName = tokens[2];
                        double weight = double.Parse(tokens[3]);
                        DateTime time = DateTime.Parse(contestDetails[1][0..11] + timeStr); // verseny napja az órán és percen kívül + fogás ideje (óra + perc)

                        Fisherman fisher = org.Search(fishermanName)!;
                        switch (fishName)
                        {
                            case "keszeg":
                                fisher.Catch(time, Bream.Instance, weight, contest);
                                break;
                            case "ponty":
                                fisher.Catch(time, Carp.Instance, weight, contest);
                                break;
                            case "harcsa":
                                fisher.Catch(time, Catfish.Instance, weight, contest);
                                break;
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Nincs ilyen file!");
            }
            return org;
        }
    }
}
