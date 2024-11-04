using Microsoft.AspNetCore.Mvc;
using azure_app_test.Services;

namespace azure_app_test.Controllers;

[Route("api/[controller]")]
[ApiController]
public class KeyVaultController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IKeyVaultSecretService keyVaultSecretService;

    public KeyVaultController(IConfiguration configuration, IKeyVaultSecretService keyVaultSecretService)
    {
        _configuration = configuration;
        this.keyVaultSecretService = keyVaultSecretService;
    }

    [HttpGet]
    public IActionResult GetAllConfigurationItems()
    {
        var response = keyVaultSecretService.GetSecrets();
        return Ok(response);
    }
}
