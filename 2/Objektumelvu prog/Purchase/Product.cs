namespace Purchase
{
    public class Product
    {
        private string _name;
        private int _price;

        public string Name => _name;
        public int Price => _price;

        public Product(string name, int price)
        {
            _name = name;
            _price = price;
        }
    }
}