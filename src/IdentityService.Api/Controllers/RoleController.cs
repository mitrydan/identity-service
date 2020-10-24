using IdentityService.Common.Constants;
using IdentityService.Models;
using IdentityService.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
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
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleController(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ApplicationRole>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRolesAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return Ok(roles);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateRoleAsync([FromBody] CreateRoleRq request)
        {
            var role = await _roleManager.FindByNameAsync(request.RoleName);
            if (role != default)
                return BadRequest("Role is already exists");

            var newRole = new ApplicationRole(request.RoleName);
            var result = await _roleManager.CreateAsync(newRole);
            return Ok(result.Succeeded);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteRoleAsync([Required] string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == default)
                return NotFound("Unable to find role");

            if (role.Name.Equals(nameof(IdentityRoles.IdentityAdmin), StringComparison.OrdinalIgnoreCase))
                return StatusCode(500, $"Unable to delete {nameof(IdentityRoles.IdentityAdmin)} role");

            var result = await _roleManager.DeleteAsync(role);
            return Ok(result.Succeeded);
        }
    }
}
