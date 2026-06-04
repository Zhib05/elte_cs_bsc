namespace Purchase
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Customer customer = CustomerFromFile("shoppinglist.txt");
                Department foods = DepartmentFromFile("foods.txt");
                Department technical = DepartmentFromFile("technical.txt");

                Store store = new(foods, technical);
                
                customer.GoShopping(store);
                Console.WriteLine(customer);

            } catch (FileNotFoundException)
            {
                Console.WriteLine("Nincs(enek) ilyen fileok");
            }
        }

        public static Customer CustomerFromFile(string filePath)
        {
            using (StreamReader sr = new (filePath))
            {
                List<string> shoppingList = new();
                while (!sr.EndOfStream)
                {
                    shoppingList.Add(sr.ReadLine()!);
                }
                return new Customer(shoppingList);
            }
        }

        public static Department DepartmentFromFile(string filePath)
        {
            using (StreamReader sr = new(filePath))
            {
                List<Product> products = new();
                while (!sr.EndOfStream)
                {
                    string[] tokens = sr.ReadLine()!.Split(' ');
                    Product p = new(tokens[0], int.Parse(tokens[1]));
                    products.Add(p);
                }
                return new Department(products);
            }
        }
    }
}
