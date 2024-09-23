using FluentValidation;
using MoneyMe.Contracts.Request;
using MoneyMe.Domain.Enums;

namespace MoneyMe.Application.Validations;

public class CalculateValidations : AbstractValidator<CalculationRequest>
{

    public CalculateValidations()
    {
        RuleFor(x => x.Product)
                .NotEmpty()
                .Must(products => ValidProducts(products ?? string.Empty))
                .WithMessage("Invalid product mus select A, B and C only");

        RuleFor(x => x.Term)
                .NotEmpty();

        RuleFor(x => x.AmountRequired)
                .NotEmpty()
                .GreaterThan(100);
    }


    private bool ValidProducts(string products)
    {
        var array = new List<string> {ProductTypes.ProductA, ProductTypes.ProductB, ProductTypes.ProductC };
        return array.Where(x=> x.Equals(products, StringComparison.Ordinal)).Any();
    }


}

