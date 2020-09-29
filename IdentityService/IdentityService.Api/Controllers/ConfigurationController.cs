using IdentityService.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static IdentityServer4.IdentityServerConstants;

namespace IdentityService.Controllers
{
    [Route("api/[controller]")]
    [Authorize(
        AuthenticationSchemes = LocalApi.AuthenticationScheme,
        Policy = LocalApi.PolicyName,
        Roles = nameof(IdentityRoles.IdentityAdmin)
    )]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        public ConfigurationController()
        {

        }
    }
}
