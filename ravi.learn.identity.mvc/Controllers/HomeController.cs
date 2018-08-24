using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ravi.learn.identity.mvc.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Globalization;
using System.Net.Http;

namespace ravi.learn.identity.mvc.Controllers
{
    public class HomeController : Controller
    {

        [Route("userinformation")]
        [Authorize]
        public IActionResult UserInformation()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [Route("[controller]/CallApi")]
        [Authorize]
        public async Task<IActionResult> CallApi()
        {
            string accessToken = null;
            ViewBag.Error = null;
            try
            {
                accessToken = await GetAccessToken();
                using (var httpClient = new HttpClient())
                {
                    httpClient.SetBearerToken(accessToken);
                    try
                    {
                        var content = await httpClient.GetStringAsync("https://localhost:44301/api/user");
                        // content = await httpClient.GetStringAsync("https://localhost:44363/resources");
                        ViewBag.ApiResponse = content;
                    }
                    catch (Exception ex)
                    {
                        ViewBag.ApiResponse = ex.GetBaseException().Message;

                    }
                }
                ViewBag.AccessToken = accessToken;
                ViewBag.RefreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.GetBaseException().Message;
                return View();
            }
        }

        private async Task<string> GetAccessToken()
        {
            var expiresAtTemp = await HttpContext.GetTokenAsync("expires_at");
            var expiresAt = DateTime.Parse(expiresAtTemp);
            if (expiresAt > DateTime.Now)
            {
                return await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            }
            return await GetRefreshedAccessToken();

        }

        private  async Task<string> GetRefreshedAccessToken()
        {
            var discoveryClient = await DiscoveryClient.GetAsync("https://localhost:44363");
            var tokenClient = new TokenClient(discoveryClient.TokenEndpoint, "WebApp", "secret");
            var refreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
            var tokenResponse = await tokenClient.RequestRefreshTokenAsync(refreshToken);
            if (!tokenResponse.IsError)
            {
                var auth = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                auth.Properties.UpdateTokenValue(OpenIdConnectParameterNames.AccessToken, tokenResponse.AccessToken);
                auth.Properties.UpdateTokenValue(OpenIdConnectParameterNames.RefreshToken, tokenResponse.RefreshToken);
                var expiresAt = DateTime.UtcNow + TimeSpan.FromSeconds(tokenResponse.ExpiresIn);
                auth.Properties.UpdateTokenValue("expires_at", expiresAt.ToString("o",CultureInfo.InvariantCulture));
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, auth.Principal, auth.Properties);
                return tokenResponse.AccessToken;
            }else
            {
                throw tokenResponse.Exception;
            }
        }
    }
}
