using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MultiTenantApi.Application.Categories.Interfaces;
using MultiTenantApi.Application.Products.Interfaces;

namespace MultiTenantApi.Application.Products;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<ProductDto>> GetProductsWithCategoryAsync()
    {
        var products = await _productRepository.GetAllAsync();
        var categories = await _categoryRepository.GetAllAsync();

        var productDtos = from p in products
            join c in categories on p.CategoryId equals c.Id
            select new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                CategoryName = c.Name
            };

        return productDtos;
    }
}
