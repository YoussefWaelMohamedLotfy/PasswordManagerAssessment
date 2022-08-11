using Elastic.Apm.NetCoreAll;
using IdentityServer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using System.Reflection;

namespace IdentityServer;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorPages();

        var serviceName = Assembly.GetCallingAssembly().GetName().Name;

        builder.Services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"), sqlOptions => sqlOptions.MigrationsAssembly(serviceName));
        });

        builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();

        builder.Services.AddIdentityServer(options =>
        {
            options.ServerSideSessions.ExpiredSessionsTriggerBackchannelLogout = true;

            options.Authentication.CoordinateClientLifetimesWithUserSession = true;

            options.EmitStaticAudienceClaim = true;

            options.Events.RaiseSuccessEvents = true;
            options.Events.RaiseFailureEvents = true;
            options.Events.RaiseErrorEvents = true;
        })
        .AddConfigurationStore(options => options.ConfigureDbContext = b
            => b.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"), opt => opt.MigrationsAssembly(serviceName)))
        .AddOperationalStore(options => options.ConfigureDbContext = b
            => b.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"), opt => opt.MigrationsAssembly(serviceName)))
        .AddAspNetIdentity<IdentityUser>()
        .AddServerSideSessions()
        .AddDeveloperSigningCredential();


        builder.Services.AddOpenTelemetryTracing(tracerProviderBuilder =>
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
                .AddAspNetCoreInstrumentation();
        });

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseAllElasticApm(app.Configuration);

        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();
        app.UseRouting();

        app.UseIdentityServer();

        app.UseAuthorization();
        app.MapRazorPages().RequireAuthorization();

        return app;
    }
}