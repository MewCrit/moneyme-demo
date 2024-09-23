

using FluentValidation;
using MeoneyMe.Application.Calculate;
using MeoneyMe.Product;
using Microsoft.Extensions.DependencyInjection;
using MoneyMe.Application.Validations;

namespace MeoneyMe.Application;

public static class DI
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IMoneyMeLoanTransaction, MoneyMeLoanTransaction>();
        services.AddScoped<IFinalLoansTransactions, FinalLoansTransactions>();
        services.AddScoped<ICalculateProduct, CalculateProduct>();
        services.AddScoped<IProductBuilder, ProductBuilder>();

        services.AddValidatorsFromAssemblyContaining<LoanRequestValidation>();
        services.AddValidatorsFromAssemblyContaining<FinalLoanValidation>();
        return services;
    }
}