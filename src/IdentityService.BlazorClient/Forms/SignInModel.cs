using System.ComponentModel.DataAnnotations;

namespace IdentityService.BlazorClient.Forms
{
    public sealed class SignInModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
