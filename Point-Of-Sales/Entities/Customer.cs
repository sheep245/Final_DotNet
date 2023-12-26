using System.Text.Json.Serialization;

namespace Point_Of_Sales.Entities
{
    public class Customer
    {
        public int Id { get; set; } 
        public required string Phone { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<Purchase>? Purchases { get; set; }
    }
}
