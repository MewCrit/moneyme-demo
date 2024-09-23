
using System.Net;
using FluentValidation;
using Mapster;
using MeoneyMe.Application.DTO;
using MeoneyMe.Infrastructure;
using Microsoft.Extensions.Logging;
using MoneyMe.Contracts.Request;
using MoneyMe.Domain;

namespace MeoneyMe.Application;

public class FinalLoansTransactions : IFinalLoansTransactions
{
    private readonly ILogger<MoneyMeLoanTransaction> _logger;

    private readonly IFinalLoanCommand _commanddapper;

    private readonly IValidator<FinalLoanRequest> _validator;

    public FinalLoansTransactions(ILogger<MoneyMeLoanTransaction> logger, 
            IFinalLoanCommand commanddapper,
            IValidator<FinalLoanRequest> validator)
    {
        this._commanddapper = commanddapper ?? throw new ArgumentNullException(nameof(commanddapper));
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this._validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<(int statusCode, FinalLoanDTO finalLoan)> CreateFinalLoanAsync(string clientLoanID, FinalLoanRequest loan)
    {
        var validation = await _validator.ValidateAsync(loan);

        if(!validation.IsValid)
        {
            this._logger.LogError("Loan has Validation Error");
            throw new ValidationException(validation.Errors);
        }

        var modelLoan = loan.Adapt<FinalLoan>();

        modelLoan.ClientLoanID = clientLoanID;
        modelLoan.TotalPayment = modelLoan.Repayment * Convert.ToInt32(modelLoan.Term);

        var result = await this._commanddapper.AddAsync(modelLoan);

        if(string.IsNullOrEmpty(result))
        {
           this._logger.LogInformation("Transaction has failed");
           return  ((int)HttpStatusCode.InternalServerError, new FinalLoanDTO( string.Empty));
        }

        this._logger.LogInformation("Returning an url");

        return ((int)HttpStatusCode.Created, new FinalLoanDTO(result));
    }

  

 
 
   
}