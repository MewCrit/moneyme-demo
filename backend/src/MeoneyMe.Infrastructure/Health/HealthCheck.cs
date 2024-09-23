
using System.Data;
using Microsoft.Extensions.Logging;

namespace MeoneyMe.Infrastructure.Health;

public class HealthCheck : IHealthCheck
{
    private ILogger<HealthCheck> _logger;

    private IDbConnection _dbConnection;

    public HealthCheck(IDbConnection dbConnection, ILogger<HealthCheck> logger) 
    {
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this._dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(logger));
    }        

    public Task<bool> Alive()
    {
        bool alive = false;
   
         _dbConnection.Open();
        this._logger.LogInformation("Succesfully connected to the database.");
        alive = true;
         _dbConnection.Close();

        return Task.FromResult(alive);
    }
}