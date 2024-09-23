
using MeoneyMe.Infrastruce;
using MeoneyMe.Infrastructure;
using MeoneyMe.Infrastructure.Health;
using MeoneyMe.Infrastructure.Query;
using Microsoft.Extensions.DependencyInjection;
using MoneyMe.Domain;

namespace MeoneyMe.Application;
public static class DI
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IHealthCheck, HealthCheck>();
        services.AddScoped<IBaseCommand<MoneyMeyLoan, string>, BaseCommand>();
        services.AddScoped<IBaseQuery<MoneyMeyLoan, string>, BaseQuery>();
        services.AddScoped<IFinalLoanCommand, FinalLoanCommand>();
        services.AddScoped<IBlacklistQuery, BlackListQuery>();

        return services;
    }
}