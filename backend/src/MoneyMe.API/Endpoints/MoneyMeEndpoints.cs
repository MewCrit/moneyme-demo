using System.Net;
using MeoneyMe.Application;
using MeoneyMe.Application.Calculate;
using MeoneyMe.Application.DTO;
using MeoneyMe.Infrastructure.Health;
using MoneyMe.Contracts;
using MoneyMe.Contracts.Request;
using MoneyMe.Contracts.Response;

namespace MoneyMe.Api.Endpoints;

public class MoneyMeEndpoints : IMoneyMeEndpoints
{
    public void RegisterEndpoints(WebApplication webAPI)
    {
        webAPI.MapGet("/health", IsHealthy);
        webAPI.MapPost("/v1/moneyme/loans", CreateClientLoan);
        webAPI.MapPost("/v1/moneyme/loans/calculations", CalculateProducts);
        webAPI.MapGet("/v1/moneyme/loans/{id}", GetUserByID);
        webAPI.MapPut("/v1/moneyme/loans/{id}", UpdateClientLoan);
        webAPI.MapPost("/v1/moneyme/loans/{id}/apply", CreateFinalLoan);

    }

    private async Task<IResult> IsHealthy(IHealthCheck health) 
    {
        var result = await health.Alive() == false ? new MoneyMeBaseResponse((int)HttpStatusCode.InternalServerError, null) : new MoneyMeBaseResponse((int)HttpStatusCode.OK, null) ; 
        return Results.Extensions.ServiceResponse(result);
    } 

    private async Task<IResult> CreateClientLoan(IMoneyMeLoanTransaction loan, ClientLoanRequest clientLoan)
    {   
        var result = await loan.CreateUserLoanAsync(clientLoan);
        return Response(result.statusCode, result.loan );
    }

    private async Task<IResult> GetUserByID(IMoneyMeLoanTransaction loan, string id)
    {   
        var result = await loan.GetUserByIdAsync(id);
        return Response(result.statusCode, result.loan );
    }

    private async Task<IResult> CalculateProducts(ICalculateProduct loan, CalculationRequest request)
    {   
        var result = await loan.CalculatePaymentViaProduct(request);
        return Response(result.statusCode,result.loan );
    }

    private async Task<IResult> UpdateClientLoan(IMoneyMeLoanTransaction loan, ClientLoanRequest clientLoan, string id)
    {
         var result = await loan.UpdateUserLoanAsync(clientLoan, id);
         return Response(result.statusCode,result.loan );
    }

    private async Task<IResult> CreateFinalLoan(IFinalLoansTransactions finalLoan,   FinalLoanRequest finalLoanRequest, string id)
    {
         var result = await finalLoan.CreateFinalLoanAsync(id, finalLoanRequest);
         return Response(result.statusCode,result.finalLoan );
    }

    private IResult Response<TModel>(int stastusCode , TModel model)
    {
        var response = new MoneyMeBaseResponse(stastusCode,  model);
        return Results.Extensions.ServiceResponse(response);
    }

} 