namespace Point
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Point> points = new List<Point>();
            Circle circle;

            using StreamReader sr = new("input.txt");

            string[] cirleParts = sr.ReadLine()!.Split(' ');
            circle = new Circle(
                new Point(
                    double.Parse(cirleParts[0]),
                    double.Parse(cirleParts[1])
                ),
                double.Parse(cirleParts[2])
            );

            int cirlesCount = int.Parse(sr.ReadLine()!);
            for (int i = 0; i < cirlesCount; i++)
            {
                string[] pointCoords = sr.ReadLine()!.Split(' ');
                points.Add(
                    new Point(
                        double.Parse(pointCoords[0]),
                        double.Parse(pointCoords[1])
                    )
                );
            }

            Console.WriteLine(circle);

            foreach (Point vasalo in points)
            {
                Console.WriteLine(vasalo);
            }

            Console.WriteLine($"A pontot: {points[3]} tartalmazza a kör: {circle.Contains(points[3])}");
            Console.WriteLine($"A pontot: {points[4]} tartalmazza a kör: {circle.Contains(points[4])}");
        }
    }
}
