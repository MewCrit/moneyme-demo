
using MoneyMe.Domain;

namespace MeoneyMe.Infrastructure;

public interface IFinalLoanCommand
{
    Task<string> AddAsync(FinalLoan model);

}
