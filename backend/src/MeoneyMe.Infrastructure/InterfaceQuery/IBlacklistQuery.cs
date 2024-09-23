

using MoneyMe.Domain;

namespace MeoneyMe.Infrastructure;

public interface IBlacklistQuery
{
    Task<bool> GetBlacklisted(string value, string type);

}
