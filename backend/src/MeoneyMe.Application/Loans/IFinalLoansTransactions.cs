

using MeoneyMe.Application.DTO;
using MoneyMe.Contracts.Request;


namespace MeoneyMe.Application;

public interface IFinalLoansTransactions
{   
    Task<(int statusCode, FinalLoanDTO finalLoan)> CreateFinalLoanAsync(string clientLoanID, FinalLoanRequest loan);

}