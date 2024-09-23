namespace MeoneyMe.Infrastructure.Health;


public interface IHealthCheck
{
    Task<bool> Alive();
}