using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using ravi.learn.identity.mvc.Services;

namespace ravi.learn.identity.mvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddMvc(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
            });

            services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = OpenIdConnectDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = "https://localhost:44363";
                    options.ClientId = "WebApp";
                    options.ClientSecret = "secret";
                    options.ResponseType = OpenIdConnectResponseType.CodeIdToken;
                    options.Scope.Add("DemoApi");
                    options.Scope.Add("profile");
                    options.Scope.Add("offline_access");
                    options.Scope.Add(ClaimTypes.DateOfBirth);
                    options.SignedOutRedirectUri = "/";
                    options.SaveTokens = true;
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters.NameClaimType = "name";
                    options.GetClaimsFromUserInfoEndpoint = true;
                })
                .AddCookie()
                .AddJwtBearer(options =>
                {
                    options.Authority = "https://localhost:44363";
                    options.Audience = "DemoApi";
                    options.TokenValidationParameters.NameClaimType = "name";

                });

            var profiles = new Dictionary<string, UserProfile>
            {
                { "BobbyZ", new UserProfile ("Bobby","Zindel", new[]{ "User" }) },
                { "RickA", new UserProfile ("Rick","Ascord", new[]{ "AdminUser" }) }
            };

            services.AddSingleton<IProfileService>(new DummyProfileService(profiles));
            services.AddSingleton<IClaimsTransformation, ProfileClaimsTransformation>();

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(CookieAuthenticationDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();

                options.AddPolicy("ageLimited", policy =>
                {
                    policy.RequireAssertion(ctx =>
                    {
                        var dob = ctx.User.FindFirstValue(ClaimTypes.DateOfBirth);
                        var dateOfBirth = DateTime.Parse(dob);
                        return (dateOfBirth < DateTime.Now.AddYears(-21));
                    });
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
