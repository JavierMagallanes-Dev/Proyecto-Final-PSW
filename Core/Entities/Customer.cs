using System;

namespace Core.Entities;

public class Customer : Entities
{
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string Address { get; set; }
    public required string Phone { get; set; }

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
