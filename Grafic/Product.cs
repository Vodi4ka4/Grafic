using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafic
{
    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Article { get; set; }
        public Product() { }

        public Product(string name, decimal price, string article)
        {
            Name = name;
            Price = price;
            Article = article; 
        }
    }
}
