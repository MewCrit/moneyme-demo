


using MeoneyMe.Application.DTO;
using MoneyMe.Contracts.Request;

namespace MeoneyMe.Application.Calculate;

public interface ICalculateProduct
{   
        Task<(int statusCode, CalculationDto loan)> CalculatePaymentViaProduct(CalculationRequest product);
}

