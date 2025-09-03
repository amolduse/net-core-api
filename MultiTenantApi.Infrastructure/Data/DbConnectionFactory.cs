using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MultiTenantApi.Application.Data;
using MultiTenantApi.Application.Data.Interfaces;
using MultiTenantApi.Application.Secrets.Interfaces;
using MultiTenantApi.Application.Tenants.Interfaces;

namespace MultiTenantApi.Infrastructure.Data;

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly ITenantContext _tenantContext;
    private readonly ISecretsManagerService _secretsManagerService;
    private readonly IConfiguration _configuration;

    public DbConnectionFactory(
        ITenantContext tenantContext,
        ISecretsManagerService secretsManagerService,
        IConfiguration configuration)
    {
        _tenantContext = tenantContext;
        _secretsManagerService = secretsManagerService;
        _configuration = configuration;
    }

    public async Task<IDbConnection> CreateConnectionAsync(DatabaseType databaseType)
    {
        var credentials = await _secretsManagerService.GetDbCredentialsAsync();
        if (credentials == null)
        {
            throw new InvalidOperationException("Database credentials could not be retrieved.");
        }

        string connectionString;
        switch (databaseType)
        {
            case DatabaseType.Config:
                connectionString = _configuration.GetConnectionString("ConfigDb");
                break;
            case DatabaseType.Shared:
                var sharedDbServer = _configuration["Database:SharedDbServer"];
                if (string.IsNullOrEmpty(sharedDbServer))
                {
                    throw new InvalidOperationException("Database:SharedDbServer is not configured.");
                }
                connectionString = new SqlConnectionStringBuilder
                {
                    DataSource = sharedDbServer,
                    InitialCatalog = "SharedDB", // Assuming a fixed name for the shared DB
                    UserID = credentials.Username,
                    Password = credentials.Password,
                    TrustServerCertificate = true // Recommended for local dev/testing
                }.ConnectionString;
                break;
            case DatabaseType.Tenant:
                if (string.IsNullOrEmpty(_tenantContext.DatabaseName))
                {
                    throw new InvalidOperationException("Tenant database name is not available.");
                }
                var tenantDbServer = _configuration["Database:TenantDbServer"];
                 if (string.IsNullOrEmpty(tenantDbServer))
                {
                    throw new InvalidOperationException("Database:TenantDbServer is not configured.");
                }
                connectionString = new SqlConnectionStringBuilder
                {
                    DataSource = tenantDbServer,
                    InitialCatalog = _tenantContext.DatabaseName,
                    UserID = credentials.Username,
                    Password = credentials.Password,
                    TrustServerCertificate = true // Recommended for local dev/testing
                }.ConnectionString;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(databaseType), databaseType, null);
        }

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Could not create a valid connection string.");
        }

        var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        return connection;
    }
}
