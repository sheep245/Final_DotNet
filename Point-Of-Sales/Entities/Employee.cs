﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Point_Of_Sales.Entities
{
    public class Employee
    {
        public int Id { get; set; }

        [ForeignKey("Account")]
        public int AccountId { get; set; }
        public virtual required Account Account { get; set; }

        public required string Fullname { get; set; }
        public required string Email { get; set; }
        public byte[]? Avatar { get; set; }

        [ForeignKey("RetailStore")]
        public int RetailStoreId { get; set; }  
        public virtual required RetailStore RetailStore { get; set; }

        public bool Status { get; set; }

        public virtual ICollection<Purchase> PurchaseHistories { get; set; }
    }
}
