namespace Core.DTOs
{
    public class CreateOrderDto
    {
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<CreateOrderItemDto> Items { get; set; }
    }

    public class CreateOrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
