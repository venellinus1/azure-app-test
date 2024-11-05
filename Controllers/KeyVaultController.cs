using Microsoft.AspNetCore.Mvc;
using azure_app_test.Services;

namespace azure_app_test.Controllers;

[Route("api/[controller]")]
[ApiController]
public class KeyVaultController : ControllerBase
{
    private readonly IConfiguration _configuration;

    private readonly IKeyVaultService keyVaultService;

    public KeyVaultController(IConfiguration configuration, IKeyVaultService keyVaultService)
    {
        _configuration = configuration;
        this.keyVaultService = keyVaultService;
    }

    [HttpGet]
    public IActionResult GetAllConfigurationItems()
    {
        var response = keyVaultService.GetSecrets();
        return Ok(response);
    }
}
