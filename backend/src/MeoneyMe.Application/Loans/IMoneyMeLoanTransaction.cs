

using MeoneyMe.Application.DTO;
using MoneyMe.Contracts.Request;


namespace MeoneyMe.Application;

public interface IMoneyMeLoanTransaction
{   
    Task<(int statusCode, LoadDTO loan)> CreateUserLoanAsync(ClientLoanRequest loan);

    Task<(int statusCode, LoadDTO loan)> UpdateUserLoanAsync(ClientLoanRequest loan, string id);

    Task<(int statusCode, ClientLoanDTO? loan)> GetUserByIdAsync(string id);
}