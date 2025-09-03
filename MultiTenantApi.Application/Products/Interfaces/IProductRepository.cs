using System.Collections.Generic;
using System.Threading.Tasks;
using MultiTenantApi.Domain.Products;

namespace MultiTenantApi.Application.Products.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
}
