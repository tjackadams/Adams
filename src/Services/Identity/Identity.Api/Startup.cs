using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Adams.Core.Extensions;
using Adams.Services.Identity.Api.Configuration;
using Adams.Services.Identity.Api.Data;
using Adams.Services.Identity.Api.Infrastructure.Filters;
using Adams.Services.Identity.Api.Models;
using HealthChecks.UI.Client;
using IdentityModel;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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
            services
                .AddCustomTelemetry(Environment, Configuration)
                .AddCustomMvc(Configuration)
                .AddHealthChecks(Configuration)
                .AddCustomSwagger(Configuration)
                .AddCustomConfiguration(Configuration)
                .AddCustomIdentityServer(Environment, Configuration);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseForwardedHeaders();

            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
                        var featurePolicy =
                            "accelerometer 'none'; camera 'none'; geolocation 'none'; gyroscope 'none'; magnetometer 'none'; microphone 'none'; payment 'none'; usb 'none'";

                        context.Context.Response.Headers["feature-policy"] = featurePolicy;
                    }

                    if (context.Context.Response.Headers["X-Content-Security-Policy"].Count == 0)
                    {
                        var csp =
                            "script-src 'self';style-src 'self';img-src 'self' data:;font-src 'self';form-action 'self';frame-ancestors 'self';block-all-mixed-content";
                        // IE
                        context.Context.Response.Headers["X-Content-Security-Policy"] = csp;
                    }
                }
            });

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthentication();
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

            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("v1/swagger.json", "Identity.API V1");
                    c.OAuthClientId("identityswaggerui");
                    c.OAuthAppName("Identity Swagger UI");
                    c.OAuthUsePkce();
                });
        }
    }

    internal static class Extensions
    {
        private static bool IsClusterEnvironment(this IConfiguration configuration)
        {
            return configuration.GetValue<string>("IsClusterEnv") == bool.TrueString;
        }

        public static IServiceCollection AddCustomTelemetry(
            this IServiceCollection services,
            IWebHostEnvironment environment,
            IConfiguration configuration
        )
        {
            if (environment.IsProduction())
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
                                    new Uri(configuration.GetValue<string>("OpenTelemetry:Zipkin:ServerUrl"));
                            });
                    });
            }

            return services;
        }

        public static IServiceCollection AddCustomMvc(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddDatabaseDeveloperPageExceptionFilter()
                .AddRouting(options => options.LowercaseUrls = true)
                .AddControllers();

            services
                .AddControllersWithViews();

            if (configuration.IsClusterEnvironment())
            {
                services.AddDataProtection(opts => { opts.ApplicationDiscriminator = "adams.identity"; })
                    .PersistKeysToStackExchangeRedis(
                        ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")),
                        "DataProtection-Keys");
            }

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthorizationConstants.AdministrationPolicy, policy =>
                {
                    policy.RequireRole(AuthorizationConstants.AdministrationRole);
                });
            });

            return services;
        }

        public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            var hcBuilder = services.AddHealthChecks();

            hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());

            hcBuilder.AddSqlServer(configuration.GetConnectionString("Identity"),
                name: "identitydb-check",
                tags: new[] {"identitydb"});

            hcBuilder.AddRedis(configuration.GetConnectionString("redis"),
                "redis-check",
                tags: new[] {"redis"});

            return services;
        }

        public static IServiceCollection AddCustomSwagger(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type => type.GetGenericTypeName());
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Adams - Identity HTTP API",
                    Version = "v1",
                    Description = "The Identity Service HTTP API"
                });
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl =
                                new Uri($"{configuration.GetValue<string>("IdentityUrl")}/connect/authorize"),
                            TokenUrl =
                                new Uri($"{configuration.GetValue<string>("IdentityUrl")}/connect/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                {"identity", "Identity API"}
                            }
                        }
                    }
                });

                options.OperationFilter<AuthorizeCheckOperationFilter>();
            });

            return services;
        }

        public static IServiceCollection AddCustomConfiguration(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddOptions();

            services.Configure<ApiBehaviorOptions>(configuration);

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Instance = context.HttpContext.Request.Path,
                        Status = StatusCodes.Status400BadRequest,
                        Detail = "Please refer to the errors property for additional details."
                    };

                    return new BadRequestObjectResult(problemDetails)
                    {
                        ContentTypes = {"application/problem+json", "application/problem+xml"}
                    };
                };
            });

            return services;
        }

        public static IServiceCollection AddCustomIdentityServer(
            this IServiceCollection services,
            IWebHostEnvironment environment,
            IConfiguration configuration
        )
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("Identity"),
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

            var connectionString = configuration.GetConnectionString("Identity");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            var identityBuilder = services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;

                    options.EmitStaticAudienceClaim = true;
                })
                .AddConfigurationStore<ApplicationConfigurationDbContext>(options =>
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

            if (environment.IsDevelopment())
            {
                identityBuilder.AddDeveloperSigningCredential();
            }
            else
            {
                identityBuilder.AddSigningCredentialsFromAzureKeyVault(options =>
                {
                    configuration.Bind("AzureKeyVault", options);
                });
            }

            services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration["IdentityUrl"];
                    options.Audience = "identity";
                })
                .AddOpenIdConnect("Azure AD / Microsoft", "Azure AD / Microsoft", options =>
                {
                    options.ClientId = configuration["External:Microsoft:ClientId"];
                    options.ClientSecret = configuration["External:Microsoft:ClientSecret"];
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
                        NameClaimType = "email"
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

            return services;
        }
    }
}