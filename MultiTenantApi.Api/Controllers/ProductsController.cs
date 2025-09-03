using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MultiTenantApi.Application.Products;
using MultiTenantApi.Application.Products.Interfaces;

namespace MultiTenantApi.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IEnumerable<ProductDto>> Get()
    {
        return await _productService.GetProductsWithCategoryAsync();
    }
}
