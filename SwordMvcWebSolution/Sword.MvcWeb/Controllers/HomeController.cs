﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Sword.MvcWeb.Models;

namespace Sword.MvcWeb.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index2()
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:7200");
            if (disco.IsError)
            {
                throw new Exception(disco.Error);
            }
            var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            client.SetBearerToken(accessToken);
            var response = await client.GetAsync("http://localhost:5001/identity");
            if (!response.IsSuccessStatusCode)
            {
                if(response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await RenewTokensAsync();
                    return RedirectToAction();
                }
                throw new Exception(response.ReasonPhrase);
            }
            var content = await response.Content.ReadAsStringAsync();
            return View("Index",content);
        }
        [Authorize]
        public async Task<IActionResult> Privacy()
        {
            var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            var idToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);

            var refreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
            // var code = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.Code);

            ViewBag.AccessToken = accessToken;
            ViewBag.IdToken = idToken;
            ViewBag.RefreshToken = refreshToken;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
            
        }


        private async Task<string> RenewTokensAsync()
        {
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:7200");
            if (disco.IsError)
            {
                throw new Exception(disco.Error);
            }

            var refreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
            var tokenResponse = await client.RequestRefreshTokenAsync(new RefreshTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "sword mvc",
                ClientSecret = "sword",
                Scope = "swordApi openid profile email phone address",
                GrantType = OpenIdConnectGrantTypes.RefreshToken,
                RefreshToken = refreshToken
            });

            if (tokenResponse.IsError)
            {
                throw new Exception(tokenResponse.Error);
            }
            var expiresAt = DateTime.UtcNow + TimeSpan.FromSeconds(tokenResponse.ExpiresIn);
            var tokens = new[]
            {
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.IdToken,
                    Value = tokenResponse.IdentityToken
                },
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.AccessToken,
                    Value = tokenResponse.AccessToken
                },
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.RefreshToken,
                    Value = tokenResponse.RefreshToken
                },
                new AuthenticationToken
                {
                    Name = "expires_at",
                    Value = expiresAt.ToString("o",CultureInfo.InvariantCulture)
                }
            };

            // 获取身份认证的结果，包含当前的pricipal和properties
            var currentAuthenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // 把新的tokens存起来
            currentAuthenticateResult.Properties.StoreTokens(tokens);
            // 登录
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, currentAuthenticateResult.Principal, currentAuthenticateResult.Properties);
            return tokenResponse.AccessToken;
        }
    }
}
