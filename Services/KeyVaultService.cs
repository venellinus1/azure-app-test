using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using azure_app_test.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace azure_app_test.Services;

public interface IKeyVaultService
{
    List<VaultSecret> GetSecrets();
    string GetSecretByName(string secretName);
}

public class KeyVaultService : IKeyVaultService
{
    private readonly SecretClient _secretClient;
    private readonly ILogger<KeyVaultService> _logger;
    private readonly Dictionary<string, string> _secretsCache;

    public KeyVaultService(IConfiguration configuration, ILogger<KeyVaultService> logger)
    {
        var keyVaultEndpoint = configuration["KeyVault:BaseUrl"];
        if (string.IsNullOrEmpty(keyVaultEndpoint))
            throw new ArgumentException("KeyVault endpoint is missing.");

        _secretClient = new SecretClient(new Uri(keyVaultEndpoint), new DefaultAzureCredential());
        _logger = logger;
        _secretsCache = new Dictionary<string, string>();

        // Fetch and cache secrets on initialization
        LoadSecretsToCache();
    }

    private void LoadSecretsToCache()
    {
        try
        {
            foreach (var secretProperty in _secretClient.GetPropertiesOfSecrets())
            {
                var secretName = secretProperty.Name;
                var secretValue = _secretClient.GetSecret(secretName).Value.Value;

                _secretsCache[secretName] = secretValue;
            }
            _logger.LogInformation("All secrets loaded into cache.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while loading secrets from Key Vault.");
        }
    }

    public List<VaultSecret> GetSecrets()
    {
        return _secretsCache.Select(kvp => new VaultSecret { Name = kvp.Key, Value = kvp.Value }).ToList();
    }

    public string GetSecretByName(string secretName)
    {
        if (_secretsCache.TryGetValue(secretName, out var secretValue))
        {
            return secretValue;
        }
        _logger.LogWarning($"Secret with name '{secretName}' was not found in cache.");
        return null;
    }
}
