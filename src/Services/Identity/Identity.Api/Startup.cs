using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Adams.Services.Identity.Api.Data;
using Adams.Services.Identity.Api.Models;
using HealthChecks.UI.Client;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using OpenTelemetry.Trace;
using Serilog;
using StackExchange.Redis;

namespace Adams.Services.Identity.Api
{
    public class Startup
    {
        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var clientUrls = new Dictionary<string, string>
            {
                {"Blazor", Configuration.GetValue<string>("BlazorClient")},
                {"SmokingApi", Configuration.GetValue<string>("SmokingApiClient")}
            };

            services.AddControllersWithViews();

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("Identity"),
                    sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                        sqlOptions.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
                    }));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.Password.RequireNonAlphanumeric = false;
                    options.SignIn.RequireConfirmedAccount = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            var connectionString = Configuration.GetConnectionString("Identity");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            var identityBuilder = services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;

                    options.EmitStaticAudienceClaim = true;
                })
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

                    options.EnableTokenCleanup = true;
                })
                .AddAspNetIdentity<ApplicationUser>();

            if (Environment.IsDevelopment())
            {
                identityBuilder.AddDeveloperSigningCredential();
            }
            else
            {
                identityBuilder.AddSigningCredentialsFromAzureKeyVault(options =>
                {
                    Configuration.Bind("AzureKeyVault", options);
                });
            }

            services.AddAuthentication()
                .AddOpenIdConnect("Azure AD / Microsoft", "Azure AD / Microsoft", options =>
                {
                    options.ClientId = Configuration["External:Microsoft:ClientId"];
                    options.ClientSecret = Configuration["External:Microsoft:ClientSecret"];
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.RemoteAuthenticationTimeout = TimeSpan.FromSeconds(30);
                    options.Authority = "https://login.microsoftonline.com/common/v2.0/";
                    options.ResponseType = "code";

                    options.UsePkce = false; // live does not support this yet

                    options.Scope.Add("profile");
                    options.Scope.Add("email");
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        NameClaimType = "email",
                    };
                    options.CallbackPath = "/signin-microsoft";
                    options.Prompt = "login";
                });

            services.Configure<CookieAuthenticationOptions>(IdentityServerConstants.DefaultCookieAuthenticationScheme,
                options =>
                {
                    options.Cookie.SameSite = SameSiteMode.None;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.IsEssential = true;
                });

            if (Configuration.GetValue<string>("IsClusterEnv") == bool.TrueString)
            {
                services.AddDataProtection(opts => { opts.ApplicationDiscriminator = "adams.identity"; })
                    .PersistKeysToStackExchangeRedis(
                        ConnectionMultiplexer.Connect(Configuration.GetConnectionString("Redis")),
                        "DataProtection-Keys");
            }

            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy())
                .AddSqlServer(Configuration.GetConnectionString("Identity"),
                    name: "identitydb-check",
                    tags: new[] {"identitydb"})
                .AddRedis(Configuration.GetConnectionString("redis"),
                    "redis-check",
                    tags: new[] {"redis"});

            if (Environment.IsProduction())
            {
                services
                    .AddOpenTelemetryTracing(builder =>
                    {
                        builder
                            .AddAspNetCoreInstrumentation(options =>
                            {
                                options.Filter = ctx =>
                                {
                                    var exclusions = ctx.RequestServices.GetRequiredService<IConfiguration>()
                                        .GetSection("OpenTelemetry:ExcludedPaths").AsEnumerable().Select(c => c.Value)
                                        .Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
                                    var path = ctx.Request.Path;
                                    if (exclusions.Contains(path))
                                    {
                                        return false;
                                    }

                                    return true;
                                };
                            })
                            .AddHttpClientInstrumentation()
                            .AddZipkinExporter(options =>
                            {
                                options.ServiceName = "identity-api";
                                options.Endpoint =
                                    new Uri(Configuration.GetValue<string>("OpenTelemetry:Zipkin:ServerUrl"));
                            });
                    });
            }
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseForwardedHeaders();

            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.Use((context, next) =>
                {
                    context.Request.Scheme = "https";
                    return next();
                });
            }

            app.UseSerilogRequestLogging();

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = context =>
                {
                    if (context.Context.Response.Headers["feature-policy"].Count == 0)
                    {
                        var featurePolicy = "accelerometer 'none'; camera 'none'; geolocation 'none'; gyroscope 'none'; magnetometer 'none'; microphone 'none'; payment 'none'; usb 'none'";

                        context.Context.Response.Headers["feature-policy"] = featurePolicy;
                    }

                    if (context.Context.Response.Headers["X-Content-Security-Policy"].Count == 0)
                    {
                        var csp = "script-src 'self';style-src 'self';img-src 'self' data:;font-src 'self';form-action 'self';frame-ancestors 'self';block-all-mixed-content";
                        // IE
                        context.Context.Response.Headers["X-Content-Security-Policy"] = csp;
                    }
                }
            });

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
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