using System;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Models;

namespace ProductCatalog.Data;

public class ProductCatalogContext : DbContext
{
    public ProductCatalogContext(DbContextOptions<ProductCatalogContext> options) : base(options){}
    
    public DbSet<Product> Products { get; set; }
}
