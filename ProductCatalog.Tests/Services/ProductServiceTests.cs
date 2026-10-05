using Moq;
using ProductCatalog.Dtos;
using ProductCatalog.Exceptions;
using ProductCatalog.Models;
using ProductCatalog.Repositories;
using ProductCatalog.Services;

namespace ProductCatalog.Tests.Services;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _repoMock;
    private readonly ProductService _service;

    public ProductServiceTests()
    {
        _repoMock = new Mock<IProductRepository>();
        _service = new ProductService(_repoMock.Object);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task CreateAsync_NomeVazio_LancaDomainException(string name)
    {
        // Arrange
        var request = new CreateProductRequest(name, 10m, 5);

        // Act + Assert
        var ex = await Assert.ThrowsAsync<DomainException>(() => _service.CreateAsync(request));
        Assert.Equal("O nome do produto é obrigatório.", ex.Message);

        _repoMock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Never);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task CreateAsync_PrecoMenorOuIgualAZero_LancaDomainException(decimal price)
    {
        var request = new CreateProductRequest("Teclado", price, 5);

        await Assert.ThrowsAsync<DomainException>(() => _service.CreateAsync(request));

        _repoMock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_EstoqueNegativo_LancaDomainException()
    {
        var request = new CreateProductRequest("Teclado", 10m, -1);

        await Assert.ThrowsAsync<DomainException>(() => _service.CreateAsync(request));
    }

    [Fact]
    public async Task CreateAsync_DadosValidos_SalvaNoRepositorioERetornaResponse()
    {
        // Arrange
        var request = new CreateProductRequest("  Teclado  ", 150.90m, 10);

        // Act
        var response = await _service.CreateAsync(request);

        // Assert
        Assert.Equal("Teclado", response.Name);
        Assert.Equal(150.90m, response.Price);
        Assert.True(response.Active);

        _repoMock.Verify(r => r.AddAsync(
            It.Is<Product>(p => p.Name == "Teclado" && p.Stock == 10)), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ProdutoInexistente_RetornaNull()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Product?)null);

        var result = await _service.GetByIdAsync(id);

        Assert.Null(result);
    }

    [Fact]
    public async Task DeactivateAsync_ProdutoExistente_DesativaESalva()
    {
        // Arrange
        var product = new Product("Mouse", 50m, 3);
        _repoMock.Setup(r => r.GetByIdAsync(product.Id)).ReturnsAsync(product);

        // Act
        var result = await _service.DeactivateAsync(product.Id);

        // Assert
        Assert.True(result);
        Assert.False(product.Active);
        _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeactivateAsync_ProdutoInexistente_RetornaFalse()
    {
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Product?)null);

        var result = await _service.DeactivateAsync(id);

        Assert.False(result);
        _repoMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }
}