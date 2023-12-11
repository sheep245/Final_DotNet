namespace Point_Of_Sales.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public required string Barcode { get; set; }
        public string Product_Name { get; set; }
        public double Import_Price { get; set; }
        public double Retail_Price { get; set;}
        public string Category { get; set; }
        public DateTime Creation_Date { get; set; }
        public Inventory Inventory { get; set; }

        public ICollection<PurchaseDetail> PurchaseDetails { get; set; }
    }
}
