using System.Security.Claims;
using Core;
using Core.DTOs;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly StoreContext _context;

        public OrdersController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    CustomerName = o.Customer.FullName,
                    Items = o.Items.Select(i => new OrderItemDto
                    {
                        ProductId = i.ProductId,
                        ProductName = i.Product.Name,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice
                    }).ToList()
                })
                .ToListAsync();

            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                .Where(o => o.Id == id)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    CustomerName = o.Customer.FullName,
                    Items = o.Items.Select(i => new OrderItemDto
                    {
                        ProductId = i.ProductId,
                        ProductName = i.Product.Name,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (order == null) return NotFound();
            return Ok(order);
        }


        [HttpPost]
public async Task<ActionResult<OrderDto>> CreateOrder(CreateOrderDto dto)
{
    var customer = await _context.Customers.FindAsync(dto.CustomerId);
    if (customer == null)
        return BadRequest(new { message = "Cliente no encontrado." });

    var order = new Order
    {
        CustomerId = dto.CustomerId,
        OrderDate = dto.OrderDate,
        Items = dto.Items.Select(i => new OrderItem
        {
            ProductId = i.ProductId,
            Quantity = i.Quantity,
            UnitPrice = i.UnitPrice
        }).ToList()
    };

    order.Total = order.Items.Sum(i => i.Quantity * i.UnitPrice);

    _context.Orders.Add(order);
    await _context.SaveChangesAsync();

    var dtoResponse = new OrderDto
    {
        Id = order.Id,
        OrderDate = order.OrderDate,
        CustomerName = customer.FullName,
        Items = order.Items.Select(i => new OrderItemDto
        {
            ProductId = i.ProductId,
            ProductName = "", // puedes llenar si haces Include de productos
            Quantity = i.Quantity,
            UnitPrice = i.UnitPrice
        }).ToList()
    };

    return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, dtoResponse);
}

[Authorize(Roles = "Cliente")]
[HttpGet("mine")]
public async Task<ActionResult<IEnumerable<OrderDto>>> GetMyOrders()
{
    var email = User.FindFirstValue(ClaimTypes.Email);
    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

    if (user == null) return Unauthorized();

    var orders = await _context.Orders
        .Include(o => o.Items)
            .ThenInclude(i => i.Product)
        .Where(o => o.CustomerId == user.Id)
        .Select(o => new OrderDto
        {
            Id = o.Id,
            OrderDate = o.OrderDate,
           CustomerName = user.Username,
            Items = o.Items.Select(i => new OrderItemDto
            {
                ProductId = i.ProductId,
                ProductName = i.Product.Name,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        })
        .ToListAsync();

    return Ok(orders);
}
    }
    
}
