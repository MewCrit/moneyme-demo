using Dapper;
using Microsoft.Extensions.Logging;
using MoneyMe.Domain;
using System.Data;

namespace MeoneyMe.Infrastructure;

public class BaseCommand : IBaseCommand<MoneyMeyLoan, string?>
{
    private ILogger<BaseCommand> _logger;

    private readonly IDbConnection _dbConnection;

    public BaseCommand(IDbConnection dbConnection, ILogger<BaseCommand> logger)
    {
         this._logger  = logger ??  throw new ArgumentNullException(nameof(logger));
         this._dbConnection  = dbConnection ??  throw new ArgumentNullException(nameof(logger));
    }

     public async Task<string?> AddAsync(MoneyMeyLoan model)
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

                 await _dbConnection.ExecuteAsync("sp_CreateClientLoan", new
                 {
                     ID = model.ID,
                     AmountRequired = model.AmountRequired,
                     Term = model.Term,
                     Title = model.Title,
                     FirstName = model.FirstName,
                     LastName = model.LastName,
                     DateOfBirth = model.DateOfBirth,
                     PhoneNumber = model.PhoneNumber,
                     Email = model.Email,
                     Product = model.Product
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

    public async Task<string?> UpdateAsync(MoneyMeyLoan model)
    {
        if (_dbConnection.State != ConnectionState.Open)
        {
            _dbConnection.Open(); 
        }

        using (var transaction = _dbConnection.BeginTransaction())
        {
             try
             {
                 await _dbConnection.ExecuteAsync("sp_UpdateClientLoan", new
                 {
                     ID = model.ID,
                     AmountRequired = model.AmountRequired,
                     Term = model.Term,
                     Title = model.Title,
                     FirstName = model.FirstName,
                     LastName = model.LastName,
                     DateOfBirth = model.DateOfBirth,
                     PhoneNumber = model.PhoneNumber,
                     Email = model.Email,
                     Product = model.Product
                 }, transaction, commandType: CommandType.StoredProcedure);

                 transaction.Commit();
                 _logger.LogInformation("Loan transaction committed");
                 return model.ID;
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
