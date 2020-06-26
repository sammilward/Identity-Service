using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace IdentityService
{
    public static class IdentityServerConfig
    {
        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>()
            {
                new ApiResource("UserAPI", "UserManagementAPI")            
                {
                    ApiSecrets = new List<Secret>
                    {
                        new Secret("usersecret".Sha256())
                    },
                    Scopes=
                    {
                        new Scope("user")
                    }
                },
                new ApiResource("RouteAPI", "RouteManagementAPI")
                {
                    ApiSecrets = new List<Secret>
                    {
                        new Secret("routesecret".Sha256())
                    },
                    Scopes=
                    {
                        new Scope("route")
                    }
                },
                new ApiResource("FriendAPI", "FriendManagementAPI")
                {
                    ApiSecrets = new List<Secret>
                    {
                        new Secret("friendsecret".Sha256())
                    },
                    Scopes=
                    {
                        new Scope("friend")
                    }
                },
                new ApiResource(IdentityServerConstants.StandardScopes.OpenId, "OpenId"),
                new ApiResource(IdentityServerConstants.StandardScopes.OfflineAccess, "OfflineAccess"),
            };
            
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>()
            {
                new Client()
                {
                    ClientId = "TheSocialAndroidMobile",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("socialtraveler".Sha256())
                    },
                    AllowedScopes =
                    {
                        "UserAPI", "RouteAPI", "FriendAPI", IdentityServerConstants.StandardScopes.OfflineAccess
                    },
                    AllowOfflineAccess = true,
                    AccessTokenLifetime = 300
                }
            };
        }
    }
}
