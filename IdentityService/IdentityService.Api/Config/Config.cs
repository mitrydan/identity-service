using IdentityServer4.Models;
using IdentityService.Common.Constants;
using System.Collections.Generic;
using static IdentityServer4.IdentityServerConstants;

namespace IdentityService.Config
{
    internal class Config
    {
        public const string RoleClaimName = "role";

        public static IEnumerable<ApiResource> GetApiResources() =>
            new List<ApiResource>
            {
                new ApiResource(LocalApi.ScopeName),
            };

        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource
                {
                    Name = RoleClaimName,
                    UserClaims = new List<string> { RoleClaimName },
                },
            };

        public static IEnumerable<ApiScope> GetApiScopes() =>
            new List<ApiScope>
            {
                new ApiScope(IdentityConstants.AdminServiceName, IdentityConstants.AdminServiceDisplayName),
                new ApiScope(LocalApi.ScopeName, LocalApi.ScopeName),
            };

        public static IEnumerable<Client> GetClients() =>
            new List<Client>
            {
                // клиент IdentityAdmin API
                new Client
                {
                    ClientId = IdentityConstants.AdminServiceName,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowOfflineAccess = true,
                    ClientSecrets =
                    {
                        new Secret(IdentityConstants.AdminServiceSecret.Sha256())
                    },
                    AllowedScopes =
                    {
                        StandardScopes.OpenId,
                        StandardScopes.Profile,
                        StandardScopes.Email,
                        RoleClaimName,
                        StandardScopes.OfflineAccess,
                        IdentityConstants.AdminServiceName,
                        LocalApi.ScopeName,
                    },
                    AlwaysSendClientClaims = true,
                    AlwaysIncludeUserClaimsInIdToken = true
                },
            };
    }
}
