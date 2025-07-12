using System;
using Core.Entities;

namespace Core.Entities;

public class OrderItem : Entities
{
    public int OrderId { get; set; }
    public Order? Order { get; set; } // 👈 no usar 'required'

    public required int ProductId { get; set; }
    public Product? Product { get; set; } // 👈 no usar 'required'

    public required int Quantity { get; set; }
    public required decimal UnitPrice { get; set; }
}
