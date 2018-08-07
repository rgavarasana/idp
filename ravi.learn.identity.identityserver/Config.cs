using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace ravi.learn.identity.identityserver
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("DemoApi", new List<string> {  "name"})
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "AuthWeb",
                    ClientName = "AuthWeb Demo Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RedirectUris = { "https://localhost:44391/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:44391/signout-callback-oidc" },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                },
                new Client
                {
                    ClientId = "WebApi",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = new[] { new  Secret("MySecret".Sha256()) },
                    AllowedScopes = new List<string>
                    {
                        "DemoApi"
                    }
                },
                new Client
                {
                    ClientId = "SPA",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "DemoApi"
                    },
                    RedirectUris = { "https://localhost:44391/SignInCallback.html" },
                    PostLogoutRedirectUris = { "https://localhost:44391/SignOutCallback.html" },
                    AllowedCorsOrigins = { "https://localhost:44391" },
                    RequireConsent = false
                },
                new Client
                {
                    ClientId  = "WebApp",
                    ClientSecrets = { new Secret("secret".Sha256())},
                    AllowedGrantTypes =  GrantTypes.Hybrid,
                    RedirectUris = { "https://localhost:44391/signin-oidc"},
                    PostLogoutRedirectUris = { "https://localhost:44391/signout-callback-oidc"},
                    AllowOfflineAccess = true,
                    RequireConsent = false,
                    AllowedScopes =  
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "DemoApi"
                    }

                }
            };
        }

        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "227BF9CC-3334-4BE7-8FA0-81A949812011",
                    Username = "BobbyZ",
                    Password = "passw0rd",
                    Claims =
                    {
                        new Claim("name", "BobbyZ"),
                        new Claim(ClaimTypes.Name, "BobbyZ"),
                        new Claim(ClaimTypes.GivenName, "Bobby"),
                        new Claim(ClaimTypes.Surname, "Zindel")
                    }
                }
            };
        }

    }
}
