using System;

namespace ProductCatalog.Models;

public class Product
{
    public Product(string name, decimal price, int stock)
    {
        Id = Guid.NewGuid();
        Name = name;
        Price = price;
        Stock = stock;
        Active = true;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; init; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public int Stock { get; private set; }
    public bool Active { get; private set; }
    public DateTime CreatedAt { get; init; }

    public void SetInactive() => Active = false;
}   
