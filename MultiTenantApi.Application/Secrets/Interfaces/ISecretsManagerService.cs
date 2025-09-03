using System.Threading.Tasks;
using MultiTenantApi.Application.Secrets;

namespace MultiTenantApi.Application.Secrets.Interfaces;

public interface ISecretsManagerService
{
    Task<DbCredentials?> GetDbCredentialsAsync();
}
