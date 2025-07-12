using System;
using Core.DTOs;
namespace Core;

public class OrderDto
{
 public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; }
        public decimal Total => Items.Sum(i => i.Quantity * i.UnitPrice);
        public List<OrderItemDto> Items { get; set; }
}
