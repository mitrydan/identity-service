using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using IdentityService.Api.Controllers.Base;
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
    public class ApiResourceController : CrudControllerBase<ConfigurationDbContext, ApiResource>
    {
        public ApiResourceController(ConfigurationDbContext configurationDbContext)
            : base(configurationDbContext)
        { }
    }
}
