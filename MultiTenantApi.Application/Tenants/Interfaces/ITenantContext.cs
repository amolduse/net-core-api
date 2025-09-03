namespace MultiTenantApi.Application.Tenants.Interfaces;

public interface ITenantContext
{
    string? TenantId { get; set; }
    string? DatabaseName { get; set; }
}
