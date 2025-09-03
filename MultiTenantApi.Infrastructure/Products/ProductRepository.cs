using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using MultiTenantApi.Application.Data;
using MultiTenantApi.Application.Data.Interfaces;
using MultiTenantApi.Application.Products.Interfaces;
using MultiTenantApi.Domain.Products;

namespace MultiTenantApi.Infrastructure.Products;

public class ProductRepository : IProductRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public ProductRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(DatabaseType.Tenant);
        return await connection.QueryAsync<Product>("SELECT * FROM Products");
    }
}
