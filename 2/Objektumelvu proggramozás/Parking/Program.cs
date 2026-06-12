namespace Parking {
    internal class Program {
        static void Main(string[] args) {
            /* | Ha inkább nem írnál magadnak saját felpopulálást, akkor használhatod ezt, de:
             * | 
             * | Nevezd át a classok és a hozzájuk tartozó metódusok, property-k
             * | neveit ebben a fileban a megvalósításod alapján! ( CTRL + H )
             * | És ne felejtsd el a namespace nevét is átírni!
             * | Checklist, hogy biztos meglegyen minden:
             * | 
             * | Zone
             * |   Zone1
             * |     .Instance
             * |   Zone2
             * |     .Instance
             * |   Zone3
             * |     .Instance
             * | Company
             * |   .Register()
             * |   .Permission()
             * |   .Where()
             * |   .Search()
             * | Vehicle 
             * |   .Start()
             * |   .End()
             * |   Car
             * |   Motorcycle
             * |   Truck
             */

            try {
                Company company = new Company(new List<Zone> { Zone1.Instance, Zone2.Instance, Zone3.Instance });

                Vehicle vehicle1 = new Car("ABC-123");
                Vehicle vehicle2 = new Motorcycle("DEF-456");
                Vehicle vehicle3 = new Truck("GHI-789");

                company.Register(vehicle1);
                company.Register(vehicle2);
                company.Register(vehicle3);

                company.Permission(vehicle1);

                vehicle1.Start(Zone1.Instance, DateTime.Now);
                vehicle2.Start(Zone2.Instance, DateTime.Now);
                vehicle3.Start(Zone2.Instance, DateTime.Now);

                Console.WriteLine("Autók parkoltatása...");

                Thread.Sleep(1000);

                vehicle1.End();
                vehicle2.End();
                vehicle3.End();

                vehicle2.Start(Zone3.Instance, DateTime.Now);

                Thread.Sleep(1000);

                vehicle2.End();
                Console.Clear();

                DateOnly dateNow = DateOnly.FromDateTime(DateTime.Now);

                company.Where(dateNow, out Zone? maxZone);
                company.Search(dateNow, out Vehicle? searchedVehicle);

                //Console.WriteLine($"Legnagyobb bevétel: {maxZone}");
                //Console.WriteLine($"Jármű ami többször is parkolt de nincs ingyenes engedélye: {searchedVehicle}");
                if (maxZone == Zone2.Instance && searchedVehicle! == vehicle2) {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("A megoldás (valószínűleg) helyes!");
                    Console.ResetColor();
                }
            } catch (Exception e) {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Az alábbi hiba érkezett:");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e);
                Console.ResetColor();
            }
        }
    }
}
