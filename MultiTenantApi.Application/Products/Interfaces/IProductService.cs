using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultiTenantApi.Application.Products.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetProductsWithCategoryAsync();
}
