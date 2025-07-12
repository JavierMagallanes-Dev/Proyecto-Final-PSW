using System;

namespace Core.DTOs;

public class OrderCreateDto
{
 
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public List<OrderItemCreateDto> Items { get; set; } = new();
    }

    public class OrderItemCreateDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

