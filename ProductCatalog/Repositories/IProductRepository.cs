using System;
using ProductCatalog.Models;

namespace ProductCatalog.Repositories;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(Guid id);
    Task AddAsync(Product product);
    Task SaveChangesAsync();
    Task DeleteAsync(Product product);

}
