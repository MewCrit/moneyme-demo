
using System.Net;
using FluentValidation;
using Mapster;
using MeoneyMe.Application.DTO;
using MeoneyMe.Infrastructure;
using MeoneyMe.Product;
using Microsoft.Extensions.Logging;
using MoneyMe.Contracts.Request;
using MoneyMe.Domain;

namespace MeoneyMe.Application;

public class MoneyMeLoanTransaction : IMoneyMeLoanTransaction
{
    private readonly ILogger<MoneyMeLoanTransaction> _logger;

    private readonly IBaseCommand<MoneyMeyLoan, string?> _commanddapper;

    private readonly IBaseQuery<MoneyMeyLoan, string> _queryDapper;

    private readonly IValidator<ClientLoanRequest> _validator;

    private readonly IProductBuilder _productBuilder;

    public MoneyMeLoanTransaction(ILogger<MoneyMeLoanTransaction> logger, 
            IBaseCommand<MoneyMeyLoan, string?> commanddapper,
            IBaseQuery<MoneyMeyLoan, string> queryDapper, 
            IValidator<ClientLoanRequest> validator,
            IProductBuilder productBuilder)
    {
        this._commanddapper = commanddapper ?? throw new ArgumentNullException(nameof(commanddapper));
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this._queryDapper = queryDapper ?? throw new ArgumentNullException(nameof(queryDapper));
        this._validator = validator ?? throw new ArgumentNullException(nameof(validator));
        this._productBuilder = productBuilder ?? throw new ArgumentNullException(nameof(productBuilder));
    }

    public async Task<(int statusCode, LoadDTO loan)> CreateUserLoanAsync(ClientLoanRequest loan) 
    {   
        var validation = await _validator.ValidateAsync(loan);

        if(!validation.IsValid)
        {
            this._logger.LogError("Loan has Validation Error");
            throw new ValidationException(validation.Errors);
        }

        var modelLoan = loan.Adapt<MoneyMeyLoan>();
        var result = await this._commanddapper.AddAsync(modelLoan);

        if(string.IsNullOrEmpty(result))
        {
           this._logger.LogInformation("Transaction has failed");
           return  ((int)HttpStatusCode.InternalServerError, new LoadDTO( "Internal server"));
        }

        this._logger.LogInformation("Returning an url");

        return ((int)HttpStatusCode.Created, new LoadDTO(result));
    }



    public async Task<(int statusCode, LoadDTO loan)> UpdateUserLoanAsync(ClientLoanRequest loan, string id)
    {
         var validation = await _validator.ValidateAsync(loan);

        if(!validation.IsValid)
        {
            this._logger.LogError("Loan has Validation Error");
            throw new ValidationException(validation.Errors);
        }

        var modelLoan = loan.Adapt<MoneyMeyLoan>();
        modelLoan.ID = id;

        var result = await this._commanddapper.UpdateAsync(modelLoan);

        if(string.IsNullOrEmpty(result))
        {
           this._logger.LogInformation("Transaction has failed");
           return  ((int)HttpStatusCode.InternalServerError, new LoadDTO( "Internal server"));
        }

        this._logger.LogInformation("Returning an url");

        return ((int)HttpStatusCode.OK, new LoadDTO(result));
    
    }
    


    public async Task<(int statusCode, ClientLoanDTO? loan)> GetUserByIdAsync(string id)
    {
        if(string.IsNullOrEmpty(id))
        {
            this._logger.LogError("ID is missing");
            throw new ArgumentNullException();
        }

        var result = await this._queryDapper.GetByIDAsync(id);
       
        if(result is null)
        {
            return  ((int)HttpStatusCode.NotFound, null);
        }

        var finalResult = result.Adapt<ClientLoanDTO>();

        var term = int.Parse(result.Term ?? string.Empty);
            this._logger.LogInformation($" Fuck you {result.Product} {term}");
        var repayment = this._productBuilder.GetInfo(result.Product ?? string.Empty, result.AmountRequired, term )
                                                .IfA()
                                                .IfB()
                                                .IfC()
                                                .BuildPayment();
        this._logger.LogInformation($" Payment {repayment} ");

        finalResult.RepaymentsFrom = repayment ;
 
        return  ((int)HttpStatusCode.OK, finalResult);
    }

   
}