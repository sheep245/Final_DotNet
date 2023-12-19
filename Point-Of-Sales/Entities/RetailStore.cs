﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Point_Of_Sales.Entities
{
    public class RetailStore
    {
        public int Id { get; set; }
        public string RetailStoreID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public virtual ICollection<Employee>? Employees { get; set; }
        [ForeignKey("Invetory")]
        public int InventoryId { get; set; }
        public virtual Inventory? Inventory { get; set; }
    }
}
