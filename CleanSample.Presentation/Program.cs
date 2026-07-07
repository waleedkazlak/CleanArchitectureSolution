using System.Reflection;
using System.Text;
using CleanSample.Application;
using CleanSample.Application.DTOs;
using CleanSample.Application.Services;
using CleanSample.Infrastructure;
using CleanSample.Infrastructure.Persistence;
using CleanSample.Presentation.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configure Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

if (builder.Environment.IsDevelopment())
{
    builder.Logging.SetMinimumLevel(LogLevel.Information);
}
else
{
    builder.Logging.SetMinimumLevel(LogLevel.Warning);
}

// Configure JWT Settings
var jwtSettings = new JwtSettingsDto
{
    SecretKey = builder.Configuration["Jwt:SecretKey"] ?? "E1CC9E8B-D819-49D8-B1C6-8CA9199F2132",
    Issuer = builder.Configuration["Jwt:Issuer"] ?? "CleanSample",
    Audience = builder.Configuration["Jwt:Audience"] ?? "CleanSampleUsers",
    ExpirationMinutes = int.TryParse(builder.Configuration["Jwt:ExpirationMinutes"], out var expMin) ? expMin : 60
};

// Add services to the container
builder.Services.AddApplication();
builder.Services.AddSingleton(jwtSettings);
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddInfrastructure(connectionString!);

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configure Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtSettings.Audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogWarning("Authentication failed: {Message}", context.Exception?.Message);
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            var username = context.Principal?.Identity?.Name;
            logger.LogInformation("Token validated for user: {Username}", username);
            return Task.CompletedTask;
        }
    };
});

// Configure Authorization
builder.Services.AddAuthorization();

// Add Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger/OpenAPI
builder.Services.AddSwaggerGen(options =>
{
    // Add document info
    var info = new OpenApiInfo
    {
        Version = "v1",
        Title = "CleanSample API",
        Description = "A clean architecture API for managing products with JWT authentication and role-based authorization.",
        Contact = new OpenApiContact
        {
            Name = "CleanSample Support",
            Email = "support@cleansample.com"
        },
        License = new OpenApiLicense
        {
            Name = "MIT"
        }
    };

    options.SwaggerDoc("v1", info);

    // Add JWT Bearer security definition
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.\r\nExample: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    // Add security requirement
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });

    // Include XML comments if available
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline - ORDER MATTERS!
if (app.Environment.IsDevelopment())
{
    // Enable Swagger UI in development
    app.UseSwagger(options =>
    {
        options.SerializeAsV2 = false;
    });

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "CleanSample API v1");
        options.DocumentTitle = "CleanSample API Documentation";
        options.DefaultModelsExpandDepth(2);
        options.DisplayRequestDuration();
        options.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Example);
    });

    // Redirect root to swagger
    app.MapGet("/", () => Results.Redirect("/swagger/index.html")).ExcludeFromDescription();
}

// Add CORS before authentication
app.UseCors("AllowAll");    

// Exception handling middleware (before logging to catch all errors)
app.UseExceptionHandling();

// Use structured logging middleware (tracks correlation IDs and performance)
app.UseStructuredLogging();

// Initialize database and seed data
try
{
    using (var scope = app.Services.CreateScope())
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("Starting database initialization");

        var context = scope.ServiceProvider.GetRequiredService<CleanSampleDbContext>();
        await context.Database.MigrateAsync();
        logger.LogInformation("Database migrated successfully");

        await DatabaseSeeder.SeedAsync(context);
        logger.LogInformation("Database seeded successfully");
    }
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during database initialization");
    throw;
}

app.UseHttpsRedirection();

// Authentication and Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Map controllers
app.MapControllers();

// Log startup information
var startupLogger = app.Services.GetRequiredService<ILogger<Program>>();
startupLogger.LogInformation("=== CleanSample API Started ===");
startupLogger.LogInformation("Environment: {Environment}", app.Environment.EnvironmentName);
startupLogger.LogInformation("Swagger UI: https://localhost:63791/swagger/index.html");
startupLogger.LogInformation("Test Credentials - Username: admin, Password: admin123");
startupLogger.LogInformation("============================");

await app.RunAsync();
