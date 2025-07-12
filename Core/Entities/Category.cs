using System;
using Core.Entities;

namespace Core.Entities;

public class Category : Entities
{
    public required string Name { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
