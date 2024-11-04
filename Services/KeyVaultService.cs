using Azure.Security.KeyVault.Secrets;
using Azure.Identity;
using azure_app_test.Models;

namespace azure_app_test.Services;

public interface IKeyVaultSecretService
{   
    List<VaultSecret> GetSecrets();
    string? GetSecretByName(string secretName);
}

public class KeyVaultSecretService : IKeyVaultSecretService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<KeyVaultSecretService> logger;
    private readonly Dictionary<string, string> _secretsCache;
    public KeyVaultSecretService(IConfiguration configuration, ILogger<KeyVaultSecretService> logger)
    {
        _configuration = configuration;
        this.logger = logger;

        _secretsCache = new Dictionary<string, string>();
        LoadSecrets();
    }    
   
    public List<VaultSecret> GetSecrets()
    {
        return _secretsCache.Select(kvp => new VaultSecret { Name = kvp.Key, Value = kvp.Value }).ToList();
    }
    
    public string? GetSecretByName(string secretName)
    {
        if (_secretsCache.TryGetValue(secretName, out var secretValue))
        {
            return secretValue;
        }
        logger.LogWarning($"Secret with name '{secretName}' was not found in cache.");
        return null;
    }

    private void LoadSecrets()
    {
        try
        {
            var keyVaultEndpoint = _configuration["KeyVault:BaseUrl"];
            var clientId = _configuration["AzureAd:ClientId"];
            var clientSecret = _configuration["AzureAd:ClientSecret"];
            var tenantId = _configuration["AzureAd:TenantId"];

            var secretClient = new SecretClient(new Uri(keyVaultEndpoint), new ClientSecretCredential(tenantId, clientId, clientSecret));

            if (!string.IsNullOrEmpty(keyVaultEndpoint))
            {
                var secrets = new Dictionary<string, string>();

                var secretProperties = secretClient.GetPropertiesOfSecrets();
                foreach (var secretProperty in secretProperties)
                {
                    var secretName = secretProperty.Name;
                    var secretValue = secretClient.GetSecret(secretName).Value.Value;
                    _secretsCache[secretName] = secretValue;                    
                }
            }
            else
            {
                logger.LogWarning("The KeyVault:BaseUrl configuration setting is missing or empty. No secrets were refreshed.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while refreshing the secrets from Key Vault.");
        }
       
    }
}
