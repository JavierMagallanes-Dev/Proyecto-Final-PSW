using System;
using Core;

namespace Core.Entities;

public class Order : Entities
{
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

  public decimal Total { get; set; } 
    public required int CustomerId { get; set; }
    public Customer? Customer { get; set; } // no required

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>(); // inicializada
}
