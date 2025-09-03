using System.Threading.Tasks;
using Dapper;
using MultiTenantApi.Application.Data;
using MultiTenantApi.Application.Data.Interfaces;
using MultiTenantApi.Application.Tenants.Interfaces;
using MultiTenantApi.Domain.Tenants;

namespace MultiTenantApi.Infrastructure.Tenants;

public class TenantRepository : ITenantRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public TenantRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<Tenant> GetByTenantIdAsync(string tenantId)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(DatabaseType.Config);
        return await connection.QuerySingleOrDefaultAsync<Tenant>(
            "SELECT * FROM Tenants WHERE TenantId = @TenantId",
            new { TenantId = tenantId });
    }
}
