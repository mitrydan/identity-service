using IdentityService.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityService.Models;
using IdentityService.Requests;
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
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ApplicationUser>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserAsync()
        {
            var usersWithRoles = await _userManager
                .Users
                .Include(x => x.UserRoles)
                    .ThenInclude(x => x.Role)
                .ToListAsync();
            return Ok(usersWithRoles);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserRq request)
        {
            var user = await _userManager.FindByNameAsync(request.UserEmail);
            if (user != default)
                return BadRequest("User is already exists");

            var newUser = new ApplicationUser
            {
                UserName = request.UserEmail,
                Email = request.UserEmail
            };
            var result = await _userManager.CreateAsync(newUser, request.Password);
            return Ok(result.Succeeded);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUserAsync([Required] string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == default)
                return NotFound("Unable to find user");

            var result = await _userManager.DeleteAsync(user);
            return Ok(result.Succeeded);
        }

        [HttpPut("{id}/applyrole")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ApplyRoleAsync([Required] string id, [FromBody] ApplyRoleRq request)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == default)
                return NotFound("Unable to find user");

            var role = await _roleManager.FindByIdAsync(request.RoleId);
            if (role == default)
                return NotFound("Unable to find role");

            var result = await _userManager.AddToRoleAsync(user, role.Name);
            return Ok(result.Succeeded);
        }

        [HttpPut("{id}/removerole")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveRoleAsync([Required] string id, [FromBody] RemoveRoleRq request)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == default)
                return NotFound("Unable to find user");

            var role = await _roleManager.FindByIdAsync(request.RoleId);
            if (role == default)
                return NotFound("Unable to find role");

            var result = await _userManager.RemoveFromRoleAsync(user, role.Name);
            return Ok(result.Succeeded);
        }
    }
}
