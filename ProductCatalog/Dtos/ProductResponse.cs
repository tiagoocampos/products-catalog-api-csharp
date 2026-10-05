using System;
using ProductCatalog.Models;

namespace ProductCatalog.Dtos;

public record ProductResponse(    
    Guid Id, string Name, decimal Price, int Stock, bool Active, DateTime CreatedAt
    )
{
    public static ProductResponse From(Product p) => 
        new ProductResponse(p.Id, p.Name, p.Price, p.Stock, p.Active, p.CreatedAt);
}
