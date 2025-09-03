using System.Data;
using System.Threading.Tasks;

namespace MultiTenantApi.Application.Data.Interfaces;

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync(DatabaseType databaseType);
}
