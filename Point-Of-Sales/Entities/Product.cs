
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Point_Of_Sales.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public required string Barcode { get; set; }
        public string Product_Name { get; set; }
        public double Import_Price { get; set; }
        public double Retail_Price { get; set; }
        public int Quantity { get; set; } = 0;
        public string Category { get; set; }
        public DateTime Creation_Date { get; set; }
        public string? ImagePath { get; set; } = null;
        public bool Is_Deleted { get; set; } = true;

        [JsonIgnore]
        public virtual ICollection<Inventory>? Inventories { get; set; }
    }
}
