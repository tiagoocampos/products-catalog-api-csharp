using ProductCatalog.Dtos;
using ProductCatalog.Exceptions;
using ProductCatalog.Models;
using ProductCatalog.Repositories;

namespace ProductCatalog.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ProductResponse>> GetAllAsync()
    {
        var products = await _repository.GetAllAsync();
        return products.Select(ProductResponse.From).ToList();
    }

    public async Task<ProductResponse?> GetByIdAsync(Guid id)
    {
        var product = await _repository.GetByIdAsync(id);
        return product is null ? null : ProductResponse.From(product);
    }

    public async Task<ProductResponse> CreateAsync(CreateProductRequest request)
    {
        Validate(request.Name, request.Price, request.Stock);

        var product = new Product(request.Name.Trim(), request.Price, request.Stock);
        await _repository.AddAsync(product);

        return ProductResponse.From(product);
    }

    public async Task<ProductResponse?> UpdateAsync(Guid id, UpdateProductRequest request)
    {
        Validate(request.Name, request.Price, request.Stock);

        var product = await _repository.GetByIdAsync(id);
        if (product is null) return null;

        product.Update(request.Name.Trim(), request.Price, request.Stock);
        await _repository.SaveChangesAsync();

        return ProductResponse.From(product);
    }

    public async Task<bool> DeactivateAsync(Guid id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product is null) return false;

        product.SetInactive();
        await _repository.SaveChangesAsync();
        return true;
    }

    private static void Validate(string name, decimal price, int stock)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("O nome do produto é obrigatório.");
        if (price <= 0)
            throw new DomainException("O preço deve ser maior que zero.");
        if (stock < 0)
            throw new DomainException("O estoque não pode ser negativo.");
    }
}