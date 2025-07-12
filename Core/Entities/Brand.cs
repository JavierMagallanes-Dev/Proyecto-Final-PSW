using System;

namespace Core.Entities;
public class Brand : Entities
{
    public required string Name { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
