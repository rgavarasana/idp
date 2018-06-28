using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Rewrite;
using ravi.learn.identity.domain.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace ravi.learn.identity.web
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
          //  JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddMvc(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());

            });

            services.AddSingleton<IUserService, DummyUserService>();

            services.AddAuthentication(options =>
                {
                    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddOpenIdConnect(options =>
                {
                    options.Authority = "https://localhost:44363";
                    options.ClientId = "AuthWeb";
                    options.SaveTokens = true;
                    options.RequireHttpsMetadata = true;
                })
                .AddCookie()
                ;


            /*  
            //var users = new Dictionary<string, string> { { "jsmith", "p@ssw0rd" }, { "lsmith", "password" } };

            //services.AddSingleton<IUserService>(new DummyUserService(users));

            
            

            

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                .AddCookie(options =>
                {
                    options.LoginPath = "/auth/signin";

                });
            

            services.AddAuthentication(options =>
            {
                options.DefaultSignInScheme = "Temporary";
                //options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddGoogle(options =>
            {
                options.ClientId = "59243158722-ickm3e2l4fioq07hinq04ikncqk5pabg.apps.googleusercontent.com";
                options.ClientSecret = "Qk2oJ8K-Z8dvR3vkBMCfUVnY";

            })
            .AddTwitter(options =>
            {
                options.ConsumerKey = "RSFDSd";
                options.ConsumerSecret = "sdsdfsdfsdf";
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/auth/signin";
            })
            .AddCookie("Temporary");
            

            services.AddAuthentication(options => {
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            })
                    .AddOpenIdConnect(options =>
                    {
                        options.Authority = "https://login.microsoftonline.com/raviaddemo123.onmicrosoft.com";
                        options.ClientId = "215eb13f-7ce9-4595-8114-d85b57a91e50";
                        options.ResponseType = OpenIdConnectResponseType.IdToken;
                        options.CallbackPath = "/auth/signin-callback";
                        options.SignedOutRedirectUri = "https://localhost:44306/";
                    })
                    .AddCookie();

        
        

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "B2C_1_sign_up_in";
            })
            .AddOpenIdConnect("B2C_1_sign_up", options => SetOptionsForOpenIdConnectPolicy("B2C_1_sign_up", options))
            .AddOpenIdConnect("B2C_1_sign_in", options => SetOptionsForOpenIdConnectPolicy("B2C_1_sign_in", options))
            .AddOpenIdConnect("B2C_1_sign_up_in", options => SetOptionsForOpenIdConnectPolicy("B2C_1_sign_up_in", options))
            .AddOpenIdConnect("B2C_1_edit_profile", options => SetOptionsForOpenIdConnectPolicy("B2C_1_edit_profile", options))
            .AddCookie();

    */

            //https://localhost:44363/



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

            app.UseRewriter((new RewriteOptions()).AddRedirectToHttps(301, 44306));

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public void SetOptionsForOpenIdConnectPolicy(string policy, OpenIdConnectOptions options)
        {
            options.MetadataAddress = "https://login.microsoftonline.com/raviadb2cdemo.onmicrosoft.com/v2.0/.well-known/openid-configuration?p=" + policy;
            options.ClientId = "ba7bb022-df3f-4b32-86a6-e4bff1f1c968";
            options.ResponseType = OpenIdConnectResponseType.IdToken;
            options.CallbackPath = "/signin/" + policy;
            options.SignedOutCallbackPath = "/signout/" + policy;
            options.SignedOutRedirectUri = "/";

        }
    }
}
