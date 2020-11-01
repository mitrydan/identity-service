using IdentityService.BlazorClient.Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace IdentityService.BlazorClient.Shared
{
    public partial class MainLayout : LayoutComponentBase
    {
        [Inject]
        private IConfiguration Configuration { get; set; }

        [Inject]
        private EventAggregator EventAggregator { get; set; }

        [Inject]
        private IIdentityServiceHttpClient HttpClient { get; set; }

        private bool IsLoggedId { get; set; }

        private string NavLinkText => IsLoggedId ? "Sign Out" : "Sign In";

        private string NavLinkHref => IsLoggedId ? "signout" : "signin";

        protected override async Task OnInitializedAsync()
        {
            //var a = await HttpClient.GetTokenAsync(new GetTokenRq
            //{
            //    ClientId = Configuration["App:ClientId"],
            //    ClientSecret = Configuration["App:ClientSecret"],
            //    GrantType = "password",
            //    Password = "QweRty_123456",
            //    Username = "admin@admin.ru"
            //});

            Console.WriteLine("asfasgfasgasgasgasg");

            var userInfo = await HttpClient.GetUserInfoAsync();
        }
    }
}
