﻿namespace CafeAssistiant.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }

        public string? ImagePath { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
