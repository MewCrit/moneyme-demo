using System.Data;
using Dapper;
using Microsoft.Extensions.Logging;
using MoneyMe.Domain;

namespace MeoneyMe.Infrastructure.Query;

public class BaseQuery : IBaseQuery<MoneyMeyLoan, string>
{
    private IDbConnection _dbConnection;

    private ILogger<BaseQuery> _logger;

    public BaseQuery(IDbConnection dbConnection, ILogger<BaseQuery> logger)
    {   
        this._dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        this._logger  = logger ??  throw new ArgumentNullException(nameof(logger));
    }   

    public async Task<MoneyMeyLoan?> GetByIDAsync(string id)
    {
       try
       {
          var result = await _dbConnection.QueryFirstOrDefaultAsync<MoneyMeyLoan>("sp_FindClientLoanByID", 
                                          new { ID = id},
                                          commandType : CommandType.StoredProcedure);

                if (result != null)
                {
                    this._logger.LogInformation("Found client: " + result.FirstName);
                    return result;
                }  
                return null;
          }
         catch (Exception ex)
          {
              this._logger.LogError("Transaction error: " + ex.Message);
              throw;
          }   
    }
}
