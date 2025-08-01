using Consul;
using Elastic.Apm.NetCoreAll;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using PasswordManager.API.Data;
using PasswordManager.API.Data.Repositories;
using PasswordManager.API.Filters;
using PasswordManager.API.Services;
using PasswordManager.Contracts;
using PasswordManager.Logging;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ConfigureEndpointDefaults(o => o.Protocols = HttpProtocols.Http1AndHttp2AndHttp3);
    options.ConfigureHttpsDefaults(o => o.AllowAnyClientCertificate());
});

builder.Host.UseSerilog(Serilogger.Configure);

// Add services to the DI container.
builder.Services.AddDbContext<AppDbContext>()
    .AddScoped<ISocialCredentialRepository, SocialCredentialRepository>();

builder.Services.AddSingleton<IEncryptionService, EncryptionService>();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = builder.Configuration.GetValue<string>("ExternalLinks:IdentityServer");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "passManagerApi");
    });
});

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

var serviceName = Assembly.GetCallingAssembly().GetName().Name;

builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder =>
{
    tracerProviderBuilder
        .AddJaegerExporter(o =>
        {
            o.AgentHost = "localhost";
            o.AgentPort = 6831;
            o.ExportProcessorType = OpenTelemetry.ExportProcessorType.Simple;
        })
        .AddSource(serviceName)
        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
        .AddHttpClientInstrumentation()
        .AddAspNetCoreInstrumentation()
        .AddEntityFrameworkCoreInstrumentation()
        .AddSqlClientInstrumentation();
});

builder.Services.AddControllers(options =>
{
    // Enables the use of "[namespace]" in Endpoint routes
    options.UseNamespaceRouteToken();
    options.Filters.Add<FluentValidationFilter>();
});

builder.Services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters()
    .AddValidatorsFromAssemblyContaining<IAssemblyScanPoint>();

builder.Services.AddSingleton<IConsulClient, ConsulClient>(_ => 
    new ConsulClient(c => c.Address = new Uri("http://raspberrypi:8500")));
builder.Services.AddHostedService<ConsulDiscoveryService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth" }
            },
            Array.Empty<string>()
        }
    });

    string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    c.UseApiEndpoints();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAllElasticApm(builder.Configuration);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers()
    .RequireAuthorization("ApiScope");

app.Run();
