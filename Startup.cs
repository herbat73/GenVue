using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using GenVue.Model;
using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Identity;
using GenVue.Services;
using GenVue.Model.InitData;
using System.Threading;
using OpenIddict.Core;
using OpenIddict.Models;
using NLog.Extensions.Logging;
using NLog.Web;
using GenVue.Middleware;
using System.IO;
using GenVue.Configuration;

namespace GenVue
{
    public class Startup
    {
        private static string ClientAppPath = "ClientApp/";

        public IHostingEnvironment CurrentEnvironment { get; protected set; }
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            this.CurrentEnvironment = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            // Nlog settings
            env.ConfigureNLog("nlog.config");

            Configuration = builder.Build();

        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            // app config read from appsettings
            services.Configure<UploadConfiguration>(
                        Configuration.GetSection("UploadSettings"));

            // Activity Log Services
            services.AddScoped<IActivityLogService, ActivityLogService>();

            services.AddDbContext<DefaultDbContext>(options =>
            {
                // database driver selection
                if (Configuration["Data:Provider"] == "postgress" )
                {
                    // Configure the context to use Postgress database.
                    // Tested on Postgress 10
                    options.UseNpgsql(Configuration["Data:DefaultConnection:ConnectionString"]);
                }
                else
                {
                    // Configure the context to use Microsoft SQL Server.
                    options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]);
                }

                // Register the entity sets needed by OpenIddict.
                // Note: use the generic overload if you need
                // to replace the default OpenIddict entities.
                options.UseOpenIddict();
            });

            // Configure Entity Framework Identity for Auth
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<DefaultDbContext>()
                .AddDefaultTokenProviders();

            // Do not 302 redirect when Unauthorized; just return 401 status code
            // see https://www.illucit.com/blog/2016/04/asp-net-5-identity-302-redirect-vs-401-unauthorized-for-api-ajax-requests/
            services.ConfigureApplicationCookie(options =>
            {
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            });

            // Configure Identity to use the same JWT claims as OpenIddict instead
            // of the legacy WS-Federation claims it uses by default (ClaimTypes),
            // which saves you from doing the mapping in your authorization controller.
            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
                options.SignIn.RequireConfirmedEmail = true;
            });

            services.AddAuthentication()
                .AddOAuthValidation();

            //// Register the OpenIddict services.
            //services.AddOpenIddict(options =>
            //{
            //    // Register the Entity Framework stores.
            //    options.AddEntityFrameworkCoreStores<DefaultDbContext>();

            //    // Register the ASP.NET Core MVC binder used by OpenIddict.
            //    // Note: if you don't call this method, you won't be able to
            //    // bind OpenIdConnectRequest or OpenIdConnectResponse parameters.
            //    options.AddMvcBinders();

            //    // Enable the authorization, logout, token and userinfo endpoints.
            //    options.EnableAuthorizationEndpoint("/api/connect/authorize")
            //           .EnableLogoutEndpoint("/api/connect/logout")
            //           .EnableTokenEndpoint("/api/connect/token")
            //           .EnableUserinfoEndpoint("/api/userinfo");

            //    // Note: the Mvc.Client sample only uses the code flow and the password flow, but you
            //    // can enable the other flows if you need to support implicit or client credentials.
            //    options.AllowAuthorizationCodeFlow()
            //           .AllowPasswordFlow()
            //           .AllowRefreshTokenFlow();

            //    // Mark the "profile" scope as a supported scope in the discovery document.
            //    options.RegisterScopes(OpenIdConnectConstants.Scopes.Profile);

            //    // Make the "client_id" parameter mandatory when sending a token request.
            //    options.RequireClientIdentification();

            //    // When request caching is enabled, authorization and logout requests
            //    // are stored in the distributed cache by OpenIddict and the user agent
            //    // is redirected to the same page with a single parameter (request_id).
            //    // This allows flowing large OpenID Connect requests even when using
            //    // an external authentication provider like Google, Facebook or Twitter.
            //    options.EnableRequestCaching();

            //    // During development, you can disable the HTTPS requirement.
            //    options.DisableHttpsRequirement();
            //});

            // Register the OpenIddict services.
            services.AddOpenIddict()
                .AddCore(options =>
                {
                    // Configure OpenIddict to use the default entities.
                    options.UseDefaultModels();

                    // Register the Entity Framework stores.
                    options.AddEntityFrameworkCoreStores<DefaultDbContext>();
                        })

                        .AddServer(options =>
                        {
                            // Register the ASP.NET Core MVC binder used by OpenIddict.
                            // Note: if you don't call this method, you won't be able to
                            // bind OpenIdConnectRequest or OpenIdConnectResponse parameters.
                            options.AddMvcBinders();

                            // Enable the authorization, logout, token and userinfo endpoints.
                            options.EnableAuthorizationEndpoint("/api/connect/authorize")
                                   .EnableLogoutEndpoint("/api/connect/logout")
                                   .EnableTokenEndpoint("/api/connect/token")
                                   .EnableUserinfoEndpoint("/api/userinfo");

                            // Allow client applications to use the code flow.
                            // Allow client applications to use the grant_type=password flow.
                            options.AllowAuthorizationCodeFlow()
                                   .AllowPasswordFlow()
                                   .AllowRefreshTokenFlow();

                            // Mark the "profile" scope as a supported scope in the discovery document.
                            options.RegisterScopes(OpenIdConnectConstants.Scopes.Profile);

                            // Make the "client_id" parameter mandatory when sending a token request.
                            options.RequireClientIdentification();

                            // When request caching is enabled, authorization and logout requests
                            // are stored in the distributed cache by OpenIddict and the user agent
                            // is redirected to the same page with a single parameter (request_id).
                            // This allows flowing large OpenID Connect requests even when using
                            // an external authentication provider like Google, Facebook or Twitter.
                            options.EnableRequestCaching();

                            // During development, you can disable the HTTPS requirement.
                            options.DisableHttpsRequirement();
                        })
                .AddValidation();

            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            // Add framework services
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();

            //add NLog.Web
            app.AddNLogWeb();

            app.UseRequestIPMiddleware();

            ClientAppPath = Path.Combine(Directory.GetCurrentDirectory(), ClientAppPath);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true,
                    ProjectPath = ClientAppPath,
                    ConfigFile = $"{ClientAppPath}webpack.config.js",
                    HotModuleReplacementEndpoint = "/dist/__webpack_hmr"
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            // Seed the database with the sample applications.
            // Note: in a real world application, this step should be part of a setup script.
            InitializeAsync(app.ApplicationServices, CancellationToken.None).GetAwaiter().GetResult();

            if (env.IsDevelopment())
            {
                // init roles and users etc
                DBInitilizer.Initialize(serviceProvider);
                GroupsInit.Initialize(serviceProvider);
                FileCategoriesInit.Initialize(serviceProvider);
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }

        private async Task InitializeAsync(IServiceProvider services, CancellationToken cancellationToken)
        {
            // Create a new service scope to ensure the database context is correctly disposed when this methods returns.
            using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DefaultDbContext>();
                await context.Database.EnsureCreatedAsync();

                var manager = scope.ServiceProvider.GetRequiredService<OpenIddictApplicationManager<OpenIddictApplication>>();

                if (await manager.FindByClientIdAsync("mvc", cancellationToken) == null)
                {
                    var descriptor = new OpenIddictApplicationDescriptor
                    {
                        ClientId = "mvc",
                        ClientSecret = "101564A5-E7FE-42CB-B10D-61EF6A8F3651",
                        DisplayName = "MVC client application",
                        PostLogoutRedirectUris = { new Uri("http://localhost:49867/signout-callback-oidc") },
                        RedirectUris = { new Uri("http://localhost:49867/signin-oidc") }
                    };

                    await manager.CreateAsync(descriptor, cancellationToken);
                }

                // To test this sample with Postman, use the following settings:
                //
                // * Authorization URL: http://localhost:49867/connect/authorize
                // * Access token URL: http://localhost:49867/connect/token
                // * Client ID: postman
                // * Client secret: [blank] (not used with public clients)
                // * Scope: openid email profile roles
                // * Grant type: authorization code
                // * Request access token locally: yes
                if (await manager.FindByClientIdAsync("postman", cancellationToken) == null)
                {
                    var descriptor = new OpenIddictApplicationDescriptor
                    {
                        ClientId = "postman",
                        DisplayName = "Postman",
                        RedirectUris = { new Uri("https://www.getpostman.com/oauth2/callback") }
                    };

                    await manager.CreateAsync(descriptor, cancellationToken);
                }
            }
        }
    }
}
