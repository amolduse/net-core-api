using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using MultiTenantApi.Application.Data;
using MultiTenantApi.Application.Data.Interfaces;
using MultiTenantApi.Application.Categories.Interfaces;
using MultiTenantApi.Domain.Categories;

namespace MultiTenantApi.Infrastructure.Categories;

public class CategoryRepository : ICategoryRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public CategoryRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(DatabaseType.Shared);
        return await connection.QueryAsync<Category>("SELECT * FROM Categories");
    }
}
