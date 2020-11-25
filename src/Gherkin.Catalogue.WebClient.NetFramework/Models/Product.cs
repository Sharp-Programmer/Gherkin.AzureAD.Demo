using System.ComponentModel;

namespace Gherkin.Catalogue.WebClient.NetFramework.Models
{
    public class Product
    {
        [DisplayName("Product Id")]
        public int Id { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Price")]
        public decimal Price { get; set; }

        [DisplayName("Available Quantity")]
        public int AvailableQuantity { get; set; }
    }
}