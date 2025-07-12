using System;

namespace Core.Entities;

public class Product : Entities
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required decimal Price { get; set; }
    public required string ImageUrl { get; set; }

    public required int BrandId { get; set; }
    public Brand? Brand { get; set; } 

    public required int CategoryId { get; set; }
    public Category? Category { get; set; } 

    public required int Stock { get; set; }
}
