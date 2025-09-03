using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using MultiTenantApi.Application.Categories.Interfaces;
using MultiTenantApi.Application.Products;
using MultiTenantApi.Application.Products.Interfaces;
using MultiTenantApi.Domain.Categories;
using MultiTenantApi.Domain.Products;

namespace MultiTenantApi.Tests;

public class ProductServiceTests
{
    [Fact]
    public async Task GetProductsWithCategoryAsync_ShouldReturnProductsWithCategoryNames()
    {
        // Arrange
        var mockProductRepo = new Mock<IProductRepository>();
        var mockCategoryRepo = new Mock<ICategoryRepository>();

        var products = new List<Product>
        {
            new() { Id = 1, Name = "Laptop", CategoryId = 1 },
            new() { Id = 2, Name = "C# in Depth", CategoryId = 2 }
        };

        var categories = new List<Category>
        {
            new() { Id = 1, Name = "Electronics" },
            new() { Id = 2, Name = "Books" }
        };

        mockProductRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);
        mockCategoryRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(categories);

        var productService = new ProductService(mockProductRepo.Object, mockCategoryRepo.Object);

        // Act
        var result = (await productService.GetProductsWithCategoryAsync()).ToList();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);

        var product1 = result.FirstOrDefault(p => p.Id == 1);
        Assert.NotNull(product1);
        Assert.Equal("Laptop", product1.Name);
        Assert.Equal("Electronics", product1.CategoryName);

        var product2 = result.FirstOrDefault(p => p.Id == 2);
        Assert.NotNull(product2);
        Assert.Equal("C# in Depth", product2.Name);
        Assert.Equal("Books", product2.CategoryName);
    }
}
