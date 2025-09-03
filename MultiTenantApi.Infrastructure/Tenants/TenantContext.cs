using MultiTenantApi.Application.Tenants.Interfaces;

namespace MultiTenantApi.Infrastructure.Tenants;

public class TenantContext : ITenantContext
{
    public string? TenantId { get; set; }
    public string? DatabaseName { get; set; }
}
