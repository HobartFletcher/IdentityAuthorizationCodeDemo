using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using SwordHybridWeb.Auths;

namespace SwordHybridWeb
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddControllersWithViews();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();


            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
             {
                 options.AccessDeniedPath = "/Authorization/AccessDenied";
             })
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
             {
                 options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                 options.Authority = "http://localhost:7200";
                 options.RequireHttpsMetadata = false;

                 options.ClientId = "hybrid client";
                 options.ClientSecret = "hybrid secret";
                 options.SaveTokens = true;
                 options.ResponseType = "code id_token"; // 这里不同授权码模式

                 options.Scope.Clear();

                 options.Scope.Add("swordApi");
                 options.Scope.Add(OidcConstants.StandardScopes.OpenId);
                 options.Scope.Add(OidcConstants.StandardScopes.Profile);
                 options.Scope.Add(OidcConstants.StandardScopes.Email);
                 options.Scope.Add(OidcConstants.StandardScopes.Phone);
                 options.Scope.Add(OidcConstants.StandardScopes.Address);
                 options.Scope.Add("roles");
                 options.Scope.Add("locations"); // 这里自定义的scope,需要再授权中心配置下 IdentityResource

                 options.Scope.Add(OidcConstants.StandardScopes.OfflineAccess);

                 // 集合里的东西，都是要被过滤掉的属性；如果remove了就不会被过滤掉；
                 options.ClaimActions.Remove("nbf");
                 options.ClaimActions.Remove("amr");
                 options.ClaimActions.Remove("exp");

                 // 不映射到 User Claims 里; 那就是添加到集合中，但方法是DeleteClaim()；
                 options.ClaimActions.DeleteClaim("sid");
                 options.ClaimActions.DeleteClaim("sub");
                 options.ClaimActions.DeleteClaim("idp");

                 // 让 Claim 里面的角色成为 mvc 系统识别的角色
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     NameClaimType = JwtClaimTypes.Name,
                     RoleClaimType = JwtClaimTypes.Role
                 };

             });


            // 策略模式
            services.AddAuthorization(options =>
            {
                //// location 是自定义的 claim , 使用它必须在授权中心配置
                //options.AddPolicy("SmithInSomewhere", builder =>
                //{
                //    builder.RequireAuthenticatedUser();
                //    builder.RequireClaim(JwtClaimTypes.FamilyName, "Smith");
                //    builder.RequireClaim("location", "somewhere");
                //});

                options.AddPolicy("SmithInSomewhere", builder =>
                {
                    builder.AddRequirements(new SmithInSomewhereRequirement());
                });
            });

            // 注入
            services.AddSingleton<IAuthorizationHandler, SmithInSomewhereHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
