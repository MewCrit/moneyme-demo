

using System.Data;
using Dapper;
using Microsoft.Extensions.Logging;
using MoneyMe.Domain;

namespace MeoneyMe.Infrastructure.Query;

public class BlackListQuery : IBlacklistQuery
{
    private readonly ILogger<BlackListQuery> _logger;

    private IDbConnection _dbConnection;


    public BlackListQuery(ILogger<BlackListQuery> logger, IDbConnection dbConnection)
    {
        this._dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        this._logger  = logger ??  throw new ArgumentNullException(nameof(logger));
    }

    public async Task<bool> GetBlacklisted(string value, string type)
    {
            try
            {
                var result = await _dbConnection.QueryFirstOrDefaultAsync<MoneyMeyLoan>("sp_ValidateBlackList", 
                                                    new { KeyValue = value, KeyType = type},
                                                    commandType : CommandType.StoredProcedure);

                if (result != null)
                {
                    this._logger.LogInformation($"The {type} is invalid");
                    return false;
                }  

                return true;

            }
            catch(Exception ex)
            {
                this._logger.LogError("Transaction error: " + ex.Message);
                throw;
            }
          
    }
}

