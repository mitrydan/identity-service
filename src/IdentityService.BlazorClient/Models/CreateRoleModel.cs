using System.ComponentModel.DataAnnotations;

namespace IdentityService.BlazorClient.Models
{
    public sealed class CreateRoleModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
