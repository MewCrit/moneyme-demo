

using System.Net;
using FluentValidation;
using MeoneyMe.Application.DTO;
using MeoneyMe.Product;
using MoneyMe.Contracts.Request;

namespace MeoneyMe.Application.Calculate;

public class CalculateProduct : ICalculateProduct
{
    private IProductBuilder _product;

    private readonly IValidator<CalculationRequest> _validator;


    public CalculateProduct(IProductBuilder product, IValidator<CalculationRequest> validator)
    {
        this._product = product ?? throw new ArgumentNullException(nameof(product));
        this._validator = validator ?? throw new ArgumentNullException(nameof(validator));

    }

    public async Task<(int statusCode, CalculationDto loan)>  CalculatePaymentViaProduct(CalculationRequest product)
    {

        var validation =  await _validator.ValidateAsync(product);

        if(!validation.IsValid)
        {
            throw new ValidationException(validation.Errors);
        }

        var repayment = _product.GetInfo(product.Product ?? string.Empty, product.AmountRequired, product.Term)
                                     .IfA()
                                     .IfB()
                                     .IfC()
                                     .BuildPayment();


        var tupleResult = ((int)HttpStatusCode.Created , new CalculationDto(repayment));

        return tupleResult;
    }
}

