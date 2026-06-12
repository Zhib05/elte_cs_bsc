using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase
{
    class Customer
    {
        private List<string> _shoppingList;
        private List<Product> _cart;

        public Customer(List<string> shoppingList)
        {
            _cart = new List<Product>();
            _shoppingList = shoppingList;
        }

        public void GoShopping(Store store)
        {
            foreach (string name in _shoppingList)
            {
                if (Search(name, store.Foods, out Product? product))
                {
                    AddToCart(product!, store.Foods);
                }
            }
            foreach (string name in _shoppingList)
            {
                if (SearchCheap(name, store.Technical, out Product? product))
                {
                    AddToCart(product!, store.Technical);
                }
            }
        }

        private static bool Search(string name, Department department, out Product? product)
        {
            product = null;

            foreach (Product item in department.Stock)
            {
                if (item.Name == name)
                {
                    product = item;
                    return true;
                }
            }
            return false;
        }

        private static bool SearchCheap(string name, Department department, out Product? product)
        {
            product = null;
            bool l = false;

            int min = 0;

            foreach (Product item in department.Stock)
            {
                if (item.Name != name)
                {
                    continue;
                }
                if (!l)
                {
                    l = true;
                    min = item.Price;
                    product = item;
                } else
                {
                    if (min > item.Price)
                    {
                        min = item.Price;
                        product = item;
                    }
                }
            }

            return l;
        }

        private void AddToCart(Product product, Department department)
        {
            department.Stock.Remove(product);
            _cart.Add(product);
        }

        public override string ToString()
        {
            string str = "Bevasarlokocsiban levo termek:";
            foreach (Product product in _cart)
            {
                str += $"{product.Name} : {product.Price}\n";
            }

            return str;
        }
    }
}
