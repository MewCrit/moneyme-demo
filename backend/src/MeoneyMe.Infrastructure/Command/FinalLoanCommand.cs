
using Dapper;
using MeoneyMe.Infrastructure;
using Microsoft.Extensions.Logging;
using MoneyMe.Domain;
using System.Data;

namespace MeoneyMe.Infrastruce;

public class FinalLoanCommand : IFinalLoanCommand
{

     private ILogger<FinalLoanCommand> _logger;

    private readonly IDbConnection _dbConnection;

    public FinalLoanCommand(IDbConnection dbConnection, ILogger<FinalLoanCommand> logger)
    {
        this._logger  = logger ??  throw new ArgumentNullException(nameof(logger));
         this._dbConnection  = dbConnection ??  throw new ArgumentNullException(nameof(logger));
    }

    public async Task<string> AddAsync(FinalLoan model)
    {
        if (_dbConnection.State != ConnectionState.Open)
        {
            _dbConnection.Open(); 
        }

        using (var transaction = _dbConnection.BeginTransaction())
        {
             try
             {
                 _logger.LogInformation("Loan transaction now in process");
                  var newID =  Ulid.NewUlid().ToString();
                  model.ID = newID;

                 await _dbConnection.ExecuteAsync("sp_CreateFinalLoan", new
                 {
                     ID = model.ID,
                     ClientLoanID = model.ClientLoanID,
                     LoanAmount = model.LoanAmount,
                     Term = model.Term,
                     Title = model.Title,
                     FirstName = model.FirstName,
                     LastName = model.LastName,
                     DateOfBirth = model.DateOfBirth,
                     PhoneNumber = model.PhoneNumber,
                     Email = model.Email,
                     Product = model.Product,
                     Repayment = model.Repayment,   
                     TotalPayment= model.TotalPayment,   

                 }, transaction, commandType: CommandType.StoredProcedure);

                 transaction.Commit();
                 _logger.LogInformation("Loan transaction committed");
                 return newID;
             }
             catch
             {
                 _logger.LogError("Transaction error");
                 transaction.Rollback();
                 throw;
             }
        }
    }
}
