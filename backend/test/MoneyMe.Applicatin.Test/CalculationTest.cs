using System.Net;
using FluentAssertions;
using FluentValidation;
using MeoneyMe.Application.Calculate;
using MeoneyMe.Product;
using MoneyMe.Contracts.Request;
using Moq;
using NUnit.Framework;

[TestFixture]
public class CalculationTest
{   
    private Mock<IProductBuilder> _mockProductBuilder;

    private Mock<IValidator<CalculationRequest>> _mockValidator;

    private CalculateProduct _calculate;
   

    [SetUp]
    public void Setup()
    {   
        _mockProductBuilder = new Mock<IProductBuilder>();
        _mockValidator = new Mock<IValidator<CalculationRequest>>();
        _calculate = new CalculateProduct(_mockProductBuilder.Object, _mockValidator.Object);
    }

        [Test]
        public Task CalculateLoan_Should_Return_ValidationError_When_Amount_Is_LessThanOrEqual_100()
        {
            var calculation = new CalculationRequest 
            { 
                AmountRequired = 100,  
                Product = "ProductA",
                Term = 12
            };
            var validationResult = new FluentValidation.Results.ValidationResult(
                new List<FluentValidation.Results.ValidationFailure> 
                { 
                    new FluentValidation.Results.ValidationFailure("AmountRequired", "'Amount Required' must be greater than 100.") 
                });

            _mockValidator.Setup(v => v.ValidateAsync(calculation, default)).ReturnsAsync(validationResult);

            var ex = Assert.ThrowsAsync<ValidationException>(async () => await _calculate.CalculatePaymentViaProduct(calculation));
            ex.Errors.Should().Contain(f => f.PropertyName == "AmountRequired" && f.ErrorMessage == "'Amount Required' must be greater than 100.");

            return Task.CompletedTask;
        }


        [Test]
        public Task CalculateLoan_Should_Return_ValidationError_When_Product_Is_Invalid()
        {
            var calculation = new CalculationRequest 
            { 
                AmountRequired = 5000,
                Product = "Underwear", 
                Term = 12
            };
            var validationResult = new FluentValidation.Results.ValidationResult(
                new List<FluentValidation.Results.ValidationFailure> 
                { 
                    new FluentValidation.Results.ValidationFailure("Product", "Invalid product must select A, B, and C only.") 
                });

            _mockValidator.Setup(v => v.ValidateAsync(calculation, default)).ReturnsAsync(validationResult);

            var ex = Assert.ThrowsAsync<ValidationException>(async () => await _calculate.CalculatePaymentViaProduct(calculation));
            ex.Errors.Should().Contain(f => f.PropertyName == "Product" && f.ErrorMessage == "Invalid product must select A, B, and C only.");

            return Task.CompletedTask;
        }


        [Test]
        public async Task CalculateLoan_Should_Return_ValidationError_When_Term_Is_Empty()
        {
            var calculation = new CalculationRequest 
            { 
                AmountRequired = 5000,
                Product = "ProductA",
                Term = 0 
            };
            var validationResult = new FluentValidation.Results.ValidationResult(
                new List<FluentValidation.Results.ValidationFailure> 
                { 
                    new FluentValidation.Results.ValidationFailure("Term", "'Term' must not be empty.") 
                });

            _mockValidator.Setup(v => v.ValidateAsync(calculation, default)).ReturnsAsync(validationResult);

            var ex = Assert.ThrowsAsync<ValidationException>(async () => await _calculate.CalculatePaymentViaProduct(calculation));
            ex.Errors.Should().Contain(f => f.PropertyName == "Term" && f.ErrorMessage == "'Term' must not be empty.");

        }

        [Test]
        public async Task CalculateLoan_Should_Return_Success_For_Product_A_When_Valid()
        {
            var calculation = new CalculationRequest 
            { 
                AmountRequired = 5000,
                Product = "ProductA",
                Term = 12
            };

            var validationResult = new FluentValidation.Results.ValidationResult();

            _mockValidator.Setup(v => v.ValidateAsync(calculation, default))
                        .ReturnsAsync(validationResult);

            _mockProductBuilder.Setup(p => p.GetInfo(calculation.Product, calculation.AmountRequired, calculation.Term))
                               .Returns(_mockProductBuilder.Object);
            _mockProductBuilder.Setup(p => p.IfA()).Returns(_mockProductBuilder.Object);
            _mockProductBuilder.Setup(p => p.IfB()).Returns(_mockProductBuilder.Object);
            _mockProductBuilder.Setup(p => p.IfC()).Returns(_mockProductBuilder.Object);
            _mockProductBuilder.Setup(p => p.BuildPayment()).Returns(416.67m);

            var result = await _calculate.CalculatePaymentViaProduct(calculation);

            result.statusCode.Should().Be((int)HttpStatusCode.Created);
            result.loan.Repayment.Should().Be(416.67m);
        }

    
}
