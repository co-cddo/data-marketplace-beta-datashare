using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Agrimetrics.DataShare.Api.Boot;
using Agrimetrics.DataShare.Api.HealthChecks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Agrimetrics.DataShare.Api;

[ExcludeFromCodeCoverage] // Cannot reliably unit test Program as it runs before DI is configured
public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();

        // Setup Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Agrimetrics DataShare", Version = "v1" });

            // OAuth2 Configuration
            const string bearer = "Bearer";
            c.AddSecurityDefinition(bearer, new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = bearer
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = bearer
                        },
                        Scheme = "oauth2",
                        Name = bearer,
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
        });

        // Define the test user flag
        var testUserEnabled = builder.Configuration.GetValue<bool>("TestUser:Enabled");

        // Configure Authentication
        const string interactiveScheme = "InteractiveScheme";
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = interactiveScheme; // Default scheme for authentication
            options.DefaultAuthenticateScheme = interactiveScheme;
            options.DefaultChallengeScheme = interactiveScheme;
        })
        .AddJwtBearer(interactiveScheme, options =>
        {
            if (testUserEnabled)
            {
                // Test user validation settings
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])), // Test user's signing key
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ClockSkew = TimeSpan.Zero
                };
            }
            else
            {
                // Normal validation settings for real users
                var secretKey = builder.Configuration["AGMJwtSettings:SecretKey"];
                var issuer = builder.Configuration["BaseUrl"];
                const string previewIssuer = "https://preview.datamarketplace.gov.uk/"; // Add preview base URL

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)), // Use the same key to sign the JWT

                    ValidateIssuer = true,
                    ValidIssuers = new[] // Allow both base URLs for issuer validation
                    {
                        issuer,
                        previewIssuer
                    },

                                ValidateAudience = true,
                                ValidAudiences = new[] // Allow both base and preview URLs for audience validation
                                {
                        $"{issuer}api",      // Base URL audience
                        $"{previewIssuer}api" // Preview URL audience
                    },

                    ValidateLifetime = true, // Ensure the token has not expired
                    ClockSkew = TimeSpan.Zero // Remove default clock skew
                };

                // Optionally handle token validation and error handling
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        var utcNow = DateTime.UtcNow;
                        var issuedAtClaim = context.Principal.FindFirstValue(JwtRegisteredClaimNames.Iat);

                        if (issuedAtClaim != null)
                        {
                            // Ensure the issued at claim is properly parsed as a Unix timestamp
                            var issuedAt = DateTimeOffset.FromUnixTimeSeconds(long.Parse(issuedAtClaim)).UtcDateTime;

                            if (issuedAt.Date != utcNow.Date)
                            {
                                context.Fail("Token was not issued today.");
                            }
                        }

                        return Task.CompletedTask;
                    }
                };
            }
        })
        .AddJwtBearer("ApiAuthScheme", options =>
        {
            // Validation for API requests
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Authentication:ApiKey"])), // Your API signing key
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration["Authentication:ApiIssuer"], // Your API issuer
                ValidateAudience = true,
                ValidAudience = builder.Configuration["Authentication:ClientId"], // Your API audience
                ClockSkew = TimeSpan.Zero // Optional: Set clock skew to zero
            };
        });

        // Configure Authorization
        builder.Services.AddAuthorization(options =>
        {
            // Policy for 'publish' scope
            options.AddPolicy("PublishScope", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "publish");
            });

            // Policy for 'discover' scope
            options.AddPolicy("DiscoverScope", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "discover");
            });

            // Policy for 'delete' scope
            options.AddPolicy("DeleteScope", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "delete");
            });

            // Generic API scope
            options.AddPolicy("ApiScope", policy =>
            {
                policy.RequireAuthenticatedUser();
            });
        });

        builder.Services.AddHttpClient();
        builder.Services.AddHttpContextAccessor();

        // Add health checks
        builder.Services.AddHealthChecks()
            .AddCheck("Data Share API Health Check", () => HealthCheckResult.Healthy("API is up and running."))
            .AddCheck<CustomSqlHealthCheck>("SQL Database Health Check", tags: new[] { "db", "sql" });

        builder.Services.AddSingleton<CustomSqlHealthCheck>(sp =>
        {
            var connectionString = builder.Configuration.GetConnectionString("sql_connection_string");
            var customSqlHealthCheckSqlCommandRunner = new CustomSqlHealthCheckSqlCommandRunner();

            return new CustomSqlHealthCheck(customSqlHealthCheckSqlCommandRunner, connectionString);
        });

        IDependencyRegistration a = new DependencyRegistration();
        a.RegisterServiceDependencies(builder.Services);

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();

        // Enable HTTPS redirection
        app.UseHttpsRedirection();

        // Apply authentication and authorization middleware
        app.UseAuthentication();
        app.UseAuthorization();

        // Map routes and health checks
        app.MapGet("/", () => $"Agrimetrics DataShare API is up and running: {DateTime.Now:yyyy/MM/dd HH:mm:ss.fff}").WithMetadata(new AllowAnonymousAttribute());

        app.MapControllers();
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";
                var result = new
                {
                    status = report.Status.ToString(),
                    totalDuration = report.TotalDuration.ToString(),
                    details = report.Entries.Select(entry => new
                    {
                        key = entry.Key,
                        status = entry.Value.Status.ToString(),
                        description = entry.Value.Description,
                        data = entry.Value.Data,
                        duration = entry.Value.Duration.ToString()
                    })
                };
                await context.Response.WriteAsJsonAsync(result, new JsonSerializerOptions { WriteIndented = true });
            }
        }).WithMetadata(new AllowAnonymousAttribute());

        // Run the application
        app.Run();
    }
}
