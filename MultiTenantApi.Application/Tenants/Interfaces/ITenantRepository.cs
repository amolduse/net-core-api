using System.Threading.Tasks;
using MultiTenantApi.Domain.Tenants;

namespace MultiTenantApi.Application.Tenants.Interfaces;

public interface ITenantRepository
{
    Task<Tenant> GetByTenantIdAsync(string tenantId);
}
