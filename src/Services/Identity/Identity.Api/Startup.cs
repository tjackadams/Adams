using System;
using System.Reflection;
using Adams.Services.Identity.Api.Data;
using Adams.Services.Identity.Api.Models;
using Adams.Services.Identity.Api.Services;
using HealthChecks.UI.Client;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Adams.Services.Identity.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Identity"),
                    sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                        //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                        sqlOptions.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
                    }));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            if (Configuration.GetValue<string>("IsClusterEnv") == bool.TrueString)
            {
                services.AddDataProtection(opts => { opts.ApplicationDiscriminator = "adams.identity"; })
                    .PersistKeysToStackExchangeRedis(ConnectionMultiplexer.Connect(Configuration.GetConnectionString("Redis")),
                        "DataProtection-Keys");
            }

            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy())
                .AddSqlServer(Configuration.GetConnectionString("Identity"),
                    name: "IdentityDB-check",
                    tags: new[] {"IdentityDB"})
                .AddRedis(Configuration.GetConnectionString("redis"),
                    name: "redis-check",
                    tags: new [] {"redis"});

            services.AddTransient<ILoginService<ApplicationUser>, LoginService>();
            services.AddTransient<IRedirectService, RedirectService>();

            var connectionString = Configuration.GetConnectionString("Identity");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            // Adds IdentityServer
            services.AddIdentityServer(x =>
                {
                    x.IssuerUri = "null";
                    x.Authentication.CookieLifetime = TimeSpan.FromHours(2);
                })
                .AddDeveloperSigningCredential()
                // .AddSigningCredential(Certificate.Get())
                .AddAspNetIdentity<ApplicationUser>()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder => builder.UseSqlServer(connectionString,
                        sqlOptions =>
                        {
                            sqlOptions.MigrationsAssembly(migrationsAssembly);
                            sqlOptions.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
                        });
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder => builder.UseSqlServer(connectionString,
                        sqlOptions =>
                        {
                            sqlOptions.MigrationsAssembly(migrationsAssembly);
                            sqlOptions.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
                        });
                })
                .Services.AddTransient<IProfileService, ProfileService>();

            services.AddControllers();
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            var pathBase = Configuration["PATH_BASE"];
            if (!string.IsNullOrEmpty(pathBase))
            {
                loggerFactory.CreateLogger<Startup>().LogDebug("Using PATH BASE '{pathBase}'", pathBase);
                app.UsePathBase(pathBase);
            }

            app.UseStaticFiles();

            app.UseForwardedHeaders();
            // Adds IdentityServer
            app.UseIdentityServer();

            app.UseCookiePolicy(new CookiePolicyOptions {MinimumSameSitePolicy = SameSiteMode.Lax});
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
                {
                    Predicate = r => r.Name.Contains("self")
                });
            });
        }
    }
}