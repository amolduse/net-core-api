using System.Text.Json;
using System.Threading.Tasks;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.Extensions.Configuration;
using MultiTenantApi.Application.Secrets;
using MultiTenantApi.Application.Secrets.Interfaces;

namespace MultiTenantApi.Infrastructure.Secrets;

public class SecretsManagerService : ISecretsManagerService
{
    private readonly IAmazonSecretsManager _secretsManager;
    private readonly IConfiguration _configuration;

    public SecretsManagerService(IAmazonSecretsManager secretsManager, IConfiguration configuration)
    {
        _secretsManager = secretsManager;
        _configuration = configuration;
    }

    public async Task<DbCredentials?> GetDbCredentialsAsync()
    {
        var secretName = _configuration["AWS:CredentialsSecretName"];
        if (string.IsNullOrEmpty(secretName))
        {
            throw new InvalidOperationException("AWS:CredentialsSecretName is not configured.");
        }

        var request = new GetSecretValueRequest
        {
            SecretId = secretName
        };

        var response = await _secretsManager.GetSecretValueAsync(request);

        if (response.SecretString != null)
        {
            return JsonSerializer.Deserialize<DbCredentials>(response.SecretString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        return null;
    }
}
