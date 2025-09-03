using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MultiTenantApi.Application.Tenants.Interfaces;

namespace MultiTenantApi.Api.Middleware;

public class TenantResolverMiddleware
{
    private readonly RequestDelegate _next;

    public TenantResolverMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ITenantContext tenantContext, ITenantRepository tenantRepository)
    {
        if (context.Request.Headers.TryGetValue("X-Tenant-Id", out var tenantIdFromHeader))
        {
            var tenantId = tenantIdFromHeader.ToString();
            var tenant = await tenantRepository.GetByTenantIdAsync(tenantId);

            if (tenant == null)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsync("Invalid Tenant ID.");
                return;
            }

            tenantContext.TenantId = tenant.TenantId;
            tenantContext.DatabaseName = tenant.DatabaseName;

            await _next(context);
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("X-Tenant-Id header is missing.");
        }
    }
}
