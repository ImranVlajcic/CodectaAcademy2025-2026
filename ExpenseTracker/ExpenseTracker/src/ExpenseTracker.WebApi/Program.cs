using ExpenseTracker.Application;
using ExpenseTracker.WebApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
{
    Directory.SetCurrentDirectory(AppContext.BaseDirectory);
    builder.Configuration
        .SetBasePath(AppContext.BaseDirectory);

    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .Filter.ByExcluding(logEvent =>
            logEvent.Properties.TryGetValue("RequestPath", out var property)
            && property.ToString().StartsWith("\"/health"))
        .CreateLogger();

    builder.Logging
        .ClearProviders()
        .AddSerilog(Log.Logger);

    builder.Host.UseSerilog();

    var jwtSettings = builder.Configuration.GetSection("Jwt");
    var secretKey = jwtSettings["Secret"] ?? throw new InvalidOperationException("JWT Secret is not configured");
    var issuer = jwtSettings["Issuer"] ?? throw new InvalidOperationException("JWT Issuer is not configured");
    var audience = jwtSettings["Audience"] ?? throw new InvalidOperationException("JWT Audience is not configured");

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false; 
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ClockSkew = TimeSpan.Zero 
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Log.Warning("Authentication failed: {Message}", context.Exception.Message);
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                var userId = context.Principal?.FindFirst("userId")?.Value;
                Log.Information("Token validated for user: {UserId}", userId);
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                Log.Warning("Authentication challenge: {Error}, {ErrorDescription}",
                    context.Error, context.ErrorDescription);
                return Task.CompletedTask;
            }
        };
    });

    builder.Services.AddAuthorization();

    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new() { Title = "ExpenseTracker API", Version = "v1" });

        
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = @"JWT Authorization header using the Bearer scheme. 
                          Enter 'Bearer' [space] and then your token in the text input below.
                          Example: 'Bearer eyJhbGc...'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });

    
    builder.Services
        .AddApplication(builder.Configuration)
        .AddInfrastructure(builder.Configuration);
}

Log.Logger.Information("Application starting");

var app = builder.Build();
{
    app.UseRouting();

    
    ServicePool.Create(app.Services);

    
    app.UseExceptionHandler("/error");

    app.UseSerilogRequestLogging(options =>
    {
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.ConfigureAuditLogging(httpContext);
        };
    });

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ExpenseTracker API v1");
        c.DocumentTitle = "ExpenseTracker API";
    });

    
    app.UseAuthentication(); 
    app.UseAuthorization();

    app.MapControllers();
}


app.Start();

Log.Logger.Information("Application started");
foreach (var url in app.Urls)
{
    Log.Logger.Information("Listening on: {url}", url);
}

app.WaitForShutdown();

Log.Logger.Information("Application shutdown gracefully");