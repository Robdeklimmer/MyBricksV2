using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyBricks.Application.Common.Interfaces;
using MyBricks.Domain.Entities;
using MyBricks.Domain.Interfaces;
using MyBricks.Infrastructure.ExternalServices.Rebrickable;
using MyBricks.Infrastructure.Identity;
using MyBricks.Infrastructure.Persistence;
using MyBricks.Infrastructure.Persistence.Repositories;
using Polly;
using Polly.Extensions.Http;

namespace MyBricks.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Persistence
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var serverVersion = ServerVersion.AutoDetect(connectionString);

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(connectionString, serverVersion,
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Repositories
        services.AddScoped<IFamilyGroupRepository, FamilyGroupRepository>();
        services.AddScoped<ILegoSetRepository, LegoSetRepository>();
        services.AddScoped<IMissingPartRepository, MissingPartRepository>();
        services.AddScoped<IUserSetRepository, UserSetRepository>();

        // Identity framework setup is handled in API layer (AddIdentityCore), 
        // but TokenService is mapped here
        services.AddScoped<ITokenService, TokenService>();

        // Rebrickable HTTP Client with Polly Retry & Circuit Breaker Policies
        services.AddHttpClient<IRebrickableClient, RebrickableClient>()
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
    }
}
