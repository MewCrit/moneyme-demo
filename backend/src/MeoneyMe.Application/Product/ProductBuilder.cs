

using Microsoft.Extensions.Logging;
using MoneyMe.Domain.Enums;

namespace MeoneyMe.Product;

public class ProductBuilder : IProductBuilder
{

    private string  Product = string.Empty;

    private decimal Loan;

    private int Terms;

    private decimal ToPay;

    private const decimal DefaultAnnualIntersetRate = 0.05m;

    private const int TwelveMonths = 12;


    private readonly ILogger<ProductBuilder> _logger;

    public ProductBuilder(ILogger<ProductBuilder> logger)
    {
        this._logger  = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public IProductBuilder GetInfo(string product, decimal loan, int terms)
    {
        Product = product;
        Loan = loan;
        Terms = terms;

        return this;
    }

    public IProductBuilder IfA()
    {
        if (Product.Equals(ProductTypes.ProductA, StringComparison.Ordinal))
        {
            _logger.LogInformation("Calculating for ProductA");

            ToPay = Loan / Terms;
        }

        return this;
    }   



    public IProductBuilder IfB()
    {
        const int SixMonth = 6; 
        const int TwoMonths = 2;

        if (Product.Equals(ProductTypes.ProductB, StringComparison.Ordinal))
        {
            _logger.LogInformation($"Calculating for ProductB {Loan} and {Terms}");

            if (Terms < SixMonth)
            {
                throw new ArgumentException("ProductB requires a minimum term of 6 months.");
            }

            var repaymentFirst2Months = Loan / Terms;
            var remainingBalance = Loan - (TwoMonths * repaymentFirst2Months);

            var monthlyInterestRate = DefaultAnnualIntersetRate / TwelveMonths;
            var remainingMonths = Terms - TwoMonths;

            var repaymentForRemainingMonths = (remainingBalance * monthlyInterestRate) /
                (1 - (decimal)Math.Pow((double)(1 + monthlyInterestRate), -remainingMonths));

            ToPay = (TwoMonths * repaymentFirst2Months) + (remainingMonths * repaymentForRemainingMonths);
        }

        return this;
    }

 
    public IProductBuilder IfC()
    {   

        const int OneMonth = 1;

         if (Product.Equals(ProductTypes.ProductC, StringComparison.Ordinal))
         {
            _logger.LogInformation("Calculating for ProductC");

             var monthlyInterestRate = DefaultAnnualIntersetRate / TwelveMonths;
             var loanInterest = Loan * monthlyInterestRate;

             ToPay =  loanInterest /  (OneMonth - (decimal)Math.Pow((double)(1 + monthlyInterestRate), -Terms));
         }

        return this;

    }

    public decimal BuildPayment()
    {
         return ToPay;
    }

}


