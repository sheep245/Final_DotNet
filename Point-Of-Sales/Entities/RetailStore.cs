namespace Point_Of_Sales.Entities
{
    public class RetailStore
    {
        public int Id { get; set; }
        public string RetailStoreID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public ICollection<Employee> Employees { get; set; }
        public ICollection<Inventory> Inventories { get; set; }
    }
}
