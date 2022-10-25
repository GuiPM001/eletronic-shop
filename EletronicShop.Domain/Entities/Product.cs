using Microsoft.AspNetCore.Http;

namespace EletronicShop.Domain.Entities
{
    public class Product : Entity
    {
        public Product(string name, int quantity, double price, string brand)
        {
            Name = name;
            Quantity = quantity;
            Price = price;
            Brand = brand;
        }

        public Product()
        {
        }

        public string Name { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string Brand { get; set; }
        public string? Image { get; set; }
    }
}
