using System;

namespace Core.DTOs;

public class ProductDto
{
public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string ImageUrl { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
}
