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
using NLog.Extensions.Logging;
using NLog.Web;
using GenVue.Configuration;
using OpenIddict.Abstractions;
using OpenIddict.EntityFrameworkCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using GenVue.CSharp.Extensions;
using GenVue.Middleware;

namespace GenVue
{
    public class Startup
    {
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
            // Add framework services
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            // Enable the Gzip compression especially for Kestrel
            services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Optimal);
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
            });

            // app config read from appsettings
            services.Configure<UploadConfiguration>(
                        Configuration.GetSection("UploadSettings"));

            // Activity Log Services
            services.AddScoped<IActivityLogService, ActivityLogService>();

            services.AddDbContext<DefaultDbContext>(options =>
            {
                // database driver selection
                switch (Configuration["Data:Provider"])
                {
                    case "postgress":
                         // Configure the context to use Postgress database.
                        // Tested on Postgress 10
                        options.UseNpgsql(Configuration["Data:DefaultConnection:ConnectionString"]);
                        break;
                    case "sqlite":
                        options.UseSqlite("Filename="+Configuration["Data:DefaultConnection:databaseName"]);
                        break;
                    case "sqlserver":
                        // Configure the context to use Microsoft SQL Server.
                        options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]);
                        break;
                    default:
                        break;
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

            services.AddOpenIddict()

            // Register the OpenIddict core services.
            .AddCore(options =>
            {
                            // Register the Entity Framework stores and models.
                            options.UseEntityFrameworkCore()
                       .UseDbContext<DefaultDbContext>();
            })

            // Register the OpenIddict server handler.
            .AddServer(options =>
            {
                            // Register the ASP.NET Core MVC binder used by OpenIddict.
                            // Note: if you don't call this method, you won't be able to
                            // bind OpenIdConnectRequest or OpenIdConnectResponse parameters.
                            options.UseMvc();

                            // Enable the authorization, logout, token and userinfo endpoints.
                            options.EnableAuthorizationEndpoint("/api/connect/authorize")
                                   .EnableLogoutEndpoint("/api/connect/logout")
                                   .EnableTokenEndpoint("/api/connect/token")
                                   .EnableUserinfoEndpoint("/api/userinfo");

                            options.AllowClientCredentialsFlow()
                                   .AllowAuthorizationCodeFlow()
                                   .AllowPasswordFlow()
                                   .AllowRefreshTokenFlow();

                            // Mark the "email", "profile" and "roles" scopes as supported scopes.
                            options.RegisterScopes(OpenIdConnectConstants.Scopes.Email,
                                                   OpenIdConnectConstants.Scopes.Profile,
                                                   OpenIddictConstants.Scopes.Roles);

                            // Accept anonymous clients (i.e clients that don't send a client_id).
                            //options.AcceptAnonymousClients();

                            // When request caching is enabled, authorization and logout requests
                            // are stored in the distributed cache by OpenIddict and the user agent
                            // is redirected to the same page with a single parameter (request_id).
                            // This allows flowing large OpenID Connect requests even when using
                            // an external authentication provider like Google, Facebook or Twitter.
                            options.EnableRequestCaching();

                            // During development, you can disable the HTTPS requirement.
                            options.DisableHttpsRequirement();

                            // Note: to use JWT access tokens instead of the default
                            // encrypted format, the following lines are required:
                            //
                            // options.UseJsonWebTokens();
                            // options.AddEphemeralSigningKey();
                        })

            // Register the OpenIddict validation handler.
            // Note: the OpenIddict validation handler is only compatible with the
            // default token format or with reference tokens and cannot be used with
            // JWT tokens. For JWT tokens, use the Microsoft JWT bearer handler.
            .AddValidation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();

            app.UseServerSentEventsMiddleware();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(
                    new WebpackDevMiddlewareOptions
                    {
                        HotModuleReplacement = true,
                        ConfigFile = "./build/webpack.config.js"
                    });
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseResponseCompression(); // No need if you use IIS, but really something good for Kestrel!

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

            // Idea: https://code.msdn.microsoft.com/How-to-fix-the-routing-225ac90f
            // This avoid having a real mvc view. You have other way of doing, but this one works
            // properly.
            app.UseSpa();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();
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
                        PostLogoutRedirectUris = { new Uri("http://localhost:53507/signout-callback-oidc") },
                        RedirectUris = { new Uri("http://localhost:53507/signin-oidc") },
                        Permissions =
                        {
                            OpenIddictConstants.Permissions.Endpoints.Authorization,
                            OpenIddictConstants.Permissions.Endpoints.Logout,
                            OpenIddictConstants.Permissions.Endpoints.Token,
                            OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                            OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                            OpenIddictConstants.Permissions.Scopes.Email,
                            OpenIddictConstants.Permissions.Scopes.Profile,
                            OpenIddictConstants.Permissions.Scopes.Roles,
                            OpenIddictConstants.Permissions.GrantTypes.Password
                        }
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
                        RedirectUris = { new Uri("https://www.getpostman.com/oauth2/callback") },
                        Permissions =
                        {
                            OpenIddictConstants.Permissions.Endpoints.Token,
                            OpenIddictConstants.Permissions.GrantTypes.Password,
                            OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                            OpenIddictConstants.Permissions.Scopes.Email,
                            OpenIddictConstants.Permissions.Scopes.Profile,
                            OpenIddictConstants.Permissions.Scopes.Roles
                        }
                    };

                    await manager.CreateAsync(descriptor, cancellationToken);
                }
            }
        }
    }
}
