using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthCenter
{
    public class CodeConfig
    {
        public static string API_RESOURCE_NAME = "swordApi";
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                new IdentityResources.Phone(),
                new IdentityResources.Email(),
                new IdentityResource("roles", "角色", new List<string>
                {
                    JwtClaimTypes.Role
                }),
                new IdentityResource("locations", "地点", new List<string>
                {
                    "location"
                })
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new ApiResource[]
            {
                new ApiResource(API_RESOURCE_NAME,"my swordApi", new List<string> {"location" })
                {
                    ApiSecrets = {new Secret("sword api secret".Sha256()) }
                }
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            string baseUrl = "http://localhost:5000";
            return new[]
            {
                // authorization code
                new Client
                {
                    ClientId = "sword mvc",
                    ClientName = "ASP.NET Core MVC sword",
                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    ClientSecrets = { new Secret("sword".Sha256()) },
                    // 登录后跳转的地址
                    RedirectUris = { "http://localhost:5000/signin-oidc" },
                    //// 基于前端注销
                    //FrontChannelLogoutUri = "http://localhost:5000/signout-oidc",
                    PostLogoutRedirectUris = { "http://localhost:5000/signout-callback-oidc" },
                    //// 基于后端注销
                    //BackChannelLogoutUri = "http://localhost:5000/signout-oidc",
                    // 将所有的claim信息放进idtoken
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowOfflineAccess = true, // offline_access
                    AccessTokenLifetime = 60, // 60 seconds 

                    AllowedScopes =
                    {
                        API_RESOURCE_NAME,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Phone,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                },
                new Client
                {
                    ClientId = "hybrid client",
                    ClientName = "ASP.NET Core MVC sword Hybrid 客户端",
                    ClientSecrets = {new Secret("hybrid secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    // 设置刷新token
                    AccessTokenType = AccessTokenType.Reference,
                    RedirectUris =
                    {
                        "http://localhost:4000/signin-oidc"
                    },
                    PostLogoutRedirectUris =
                    {
                        "http://localhost:4000/signout-callback-oidc"
                    },
                    AllowOfflineAccess = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowedScopes =
                    {
                        API_RESOURCE_NAME,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Phone,
                        IdentityServerConstants.StandardScopes.Profile,
                        "roles",
                        "locations"
                    }
                }
            };
        }


        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>() {
                new TestUser
                {
                    SubjectId = "123456",
                    Username = "fletcher",
                    Password = "fletcher",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Name,"fletcher f"),
                        new Claim(JwtClaimTypes.GivenName,"fletcher"),
                        new Claim(JwtClaimTypes.FamilyName,"f"),
                        new Claim(JwtClaimTypes.Email,"fletcher@email.com"),
                        new Claim(JwtClaimTypes.EmailVerified,"true",ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite,"http://fletcher.com"),
                        new Claim(JwtClaimTypes.Address,@"{ 'street_address': 'fff', 'locality': 'zhong', 'postal_code': 123555, 'country': 'cn' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
                        new Claim("location", "somewhere"),
                        new Claim(JwtClaimTypes.Role, "管理员")
                    }
                },
                new TestUser
                {
                    SubjectId = "789000",
                    Username = "hobart",
                    Password = "hobart",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Name,"hobart h"),
                        new Claim(JwtClaimTypes.GivenName,"hobart"),
                        new Claim(JwtClaimTypes.FamilyName,"h"),
                        new Claim(JwtClaimTypes.Email,"hobart@email.com"),
                        new Claim(JwtClaimTypes.EmailVerified,"true",ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite,"http://hobart.com"),
                        new Claim(JwtClaimTypes.Address,@"{ 'street_address': 'hhh', 'locality': 'zhong', 'postal_code': 667899, 'country': 'cn' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
                        new Claim("location", "somewhere22"),
                        new Claim(JwtClaimTypes.Role, "普通用户")
                    }
                }
            };
        }
    }
}
