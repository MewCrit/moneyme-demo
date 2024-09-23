using Moq;
using NUnit.Framework;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using MeoneyMe.Application;
using MoneyMe.Contracts.Request;
using MoneyMe.Domain;
using System.Net;
using MeoneyMe.Infrastructure;
using MeoneyMe.Product;
using MoneyMe.Domain.Enums;
using MoneyMe.Application.Validations;

[TestFixture]
public class MoneyMeLoanTest
{
    private Mock<ILogger<MoneyMeLoanTransaction>> _mockLogger;
    private Mock<IBaseCommand<MoneyMeyLoan, string?>> _mockCommandDapper;
    private Mock<IBaseQuery<MoneyMeyLoan, string>> _mockQueryDapper;
    private Mock<IValidator<ClientLoanRequest>> _mockValidator;
    private Mock<IProductBuilder> _mockProductBuilder;
    private MoneyMeLoanTransaction _transaction;
    private Mock<IBlacklistQuery> _mockBlacklist;

    private LoanRequestValidation _validator;


    [SetUp]
    public void Setup()
    {
        _mockLogger = new Mock<ILogger<MoneyMeLoanTransaction>>();
        _mockCommandDapper = new Mock<IBaseCommand<MoneyMeyLoan, string?>>();
        _mockQueryDapper = new Mock<IBaseQuery<MoneyMeyLoan, string>>();
        _mockValidator = new Mock<IValidator<ClientLoanRequest>>();
        _mockProductBuilder = new Mock<IProductBuilder>();

        _mockBlacklist = new Mock<IBlacklistQuery>();
        _validator = new LoanRequestValidation(_mockBlacklist.Object);

        _transaction = new MoneyMeLoanTransaction(
            _mockLogger.Object,
            _mockCommandDapper.Object,
            _mockQueryDapper.Object,
            _mockValidator.Object,
            _mockProductBuilder.Object
        );
    }

    [Test]
    public async Task CreateUserLoan_Should_Return_Created_When_Valid()
    {
        var loanRequest = new ClientLoanRequest { FirstName = "Sharpedo", LastName = "Wailord", Product = "ProductA" };
        var validationResult = new FluentValidation.Results.ValidationResult();
        _mockValidator.Setup(v => v.ValidateAsync(loanRequest, default)).ReturnsAsync(validationResult);
        _mockCommandDapper.Setup(c => c.AddAsync(It.IsAny<MoneyMeyLoan>())).ReturnsAsync("loan-123");

        var stubProductBuilder = new StubProductBuilder(5000);

        _transaction = new MoneyMeLoanTransaction(
            _mockLogger.Object,
            _mockCommandDapper.Object,
            _mockQueryDapper.Object,
            _mockValidator.Object,
            stubProductBuilder
        );

        var result = await _transaction.CreateUserLoanAsync(loanRequest);

        result.statusCode.Should().Be((int)HttpStatusCode.Created);
        _mockCommandDapper.Verify(c => c.AddAsync(It.IsAny<MoneyMeyLoan>()), Times.Once);

        _mockLogger.Verify(
            x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Returning an url") ),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
            Times.Once);
    }


    [Test]
    public  Task CreateUserLoan_Should_Return_ValidationError_When_FirstName_Is_Missing()
    {
        var loanRequest = new ClientLoanRequest { FirstName = "", LastName = "Wailord", Product = "ProductA" };
        var validationResult = new FluentValidation.Results.ValidationResult(
            new List<FluentValidation.Results.ValidationFailure> 
            { 
                new FluentValidation.Results.ValidationFailure("FirstName", "First name is required") 
            });

        _mockValidator.Setup(v => v.ValidateAsync(loanRequest, default)).ReturnsAsync(validationResult);

        var ex = Assert.ThrowsAsync<ValidationException>(async () => await _transaction.CreateUserLoanAsync(loanRequest));
        ex.Errors.Should().Contain(f => f.PropertyName == "FirstName" && f.ErrorMessage == "First name is required");
        _mockCommandDapper.Verify(c => c.AddAsync(It.IsAny<MoneyMeyLoan>()), Times.Never);
        return Task.CompletedTask;

    }


    [Test]
    public  Task CreateUserLoan_Should_Return_ValidationError_When_Product_Does_Not_Belong_In_The_Category()
    {
        var loanRequest = new ClientLoanRequest { FirstName = "Sharpedo", LastName = "Wailord", Product = "ProductD" };
        var validationResult = new FluentValidation.Results.ValidationResult(
            new List<FluentValidation.Results.ValidationFailure> 
            { 
                new FluentValidation.Results.ValidationFailure("Product", "Invalid product must select A, B, and C only"),
            });

        _mockValidator.Setup(v => v.ValidateAsync(loanRequest, default)).ReturnsAsync(validationResult);

        var ex =  Assert.ThrowsAsync<ValidationException>(async () =>  await _transaction.CreateUserLoanAsync(loanRequest));

        ex.Errors.Should().Contain(f => f.PropertyName == "Product" && f.ErrorMessage == "Invalid product must select A, B, and C only");

        _mockCommandDapper.Verify(c => c.AddAsync(It.IsAny<MoneyMeyLoan>()), Times.Never);

        return Task.CompletedTask;
    }


    [Test]
     public   Task CreateUserLoan_Should_Return_ValidationError_When_PhoneNumber_Does_Not_Meet_The_Requirement()
     {

        var loanRequest = new ClientLoanRequest { FirstName = "Sharpedo", LastName = "Wailord", Product = "ProductA", PhoneNumber="123", };
        var validationResult = new FluentValidation.Results.ValidationResult(
            new List<FluentValidation.Results.ValidationFailure> 
            { 
                new FluentValidation.Results.ValidationFailure("PhoneNumber", "This phone number length not allowed"),
            });
        _mockValidator.Setup(v => v.ValidateAsync(loanRequest, default)).ReturnsAsync(validationResult);
          var ex =  Assert.ThrowsAsync<ValidationException>(async () =>  await _transaction.CreateUserLoanAsync(loanRequest));
        ex.Errors.Should().Contain(f => f.PropertyName == "PhoneNumber" && f.ErrorMessage == "This phone number length not allowed");

        _mockCommandDapper.Verify(c => c.AddAsync(It.IsAny<MoneyMeyLoan>()), Times.Never);
        return Task.CompletedTask;
     }

    [Test]
    public async Task CreateUserLoan_Should_FailValidation_When_Contacts_Are_Blacklisted()
    {
        var loanRequest = new ClientLoanRequest
        {
            FirstName = "Sharpedo",
            LastName = "Wailord",
            Email = "sarpedo@fuzzy.com", 
            PhoneNumber = "+63917683158", 
            DateOfBirth = DateTime.Now.AddYears(-20),
            AmountRequired = 5000,
            Product = "ProductA"
        };

        _mockBlacklist.Setup(b => b.GetBlacklisted("fuzzy.com", ContactTypes.EmailDomain))
            .ReturnsAsync(false);

        _mockBlacklist.Setup(b => b.GetBlacklisted(It.IsAny<string>(), ContactTypes.MobileNumber))
            .ReturnsAsync(false);

        var result = await _validator.ValidateAsync(loanRequest);

        result.IsValid.Should().BeFalse();
        foreach (var error in result.Errors)
        {
            Console.WriteLine($"Error: PropertyName: {error.PropertyName}, ErrorMessage: {error.ErrorMessage}");
        }

        result.Errors.Should().Contain(f => f.PropertyName == "PhoneNumber" && f.ErrorMessage == "This phone number is not allowed.");
        result.Errors.Should().Contain(f => f.PropertyName == "Email" && f.ErrorMessage == "This email domain is not allowed.");

    }

    [Test]
    public Task CreateUserLoan_Should_FailValidation_If_user_Is_Under_Eighteen()
    {
        var loanRequest = new ClientLoanRequest { FirstName = "Sharpedo", LastName = "Wailord", Product = "ProductA", DateOfBirth=DateTime.Now.AddYears(-10), };
        var validationResult = new FluentValidation.Results.ValidationResult(
        new List<FluentValidation.Results.ValidationFailure> 
        { 
            new FluentValidation.Results.ValidationFailure("DateOfBirth", "Client must be 18 years old and above"),
        });

        _mockValidator.Setup(v => v.ValidateAsync(loanRequest, default)).ReturnsAsync(validationResult);
        var ex =  Assert.ThrowsAsync<ValidationException>(async () =>  await _transaction.CreateUserLoanAsync(loanRequest));
        ex.Errors.Should().Contain(f => f.PropertyName == "DateOfBirth" && f.ErrorMessage == "Client must be 18 years old and above");

        _mockCommandDapper.Verify(c => c.AddAsync(It.IsAny<MoneyMeyLoan>()), Times.Never);
        return Task.CompletedTask;
    }

    [Test]
    public async Task Retrieve_Loan_Using_The_ID_Loan_Should_Return_OK()
    {
        var loanID = "01J87VT29KD6CBY53208DJWV69";
    
            var mockLoan = new MoneyMeyLoan
            {
                ID = loanID,
                AmountRequired = 5000,
                Term = "24",
                Title = "Mr.",
                FirstName = "Sharpedo",
                LastName = "Wailord",
                DateOfBirth = DateTime.Parse("1980-01-01"),
                PhoneNumber = "1234567890",
                Email = "sharpedo@robonsons.com.ph",
                Product = "ProductA"
            };

            _mockQueryDapper.Setup(q => q.GetByIDAsync(loanID)).ReturnsAsync(mockLoan);

            _mockProductBuilder.Setup(p => p.GetInfo(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<int>())).Returns(_mockProductBuilder.Object);

            _mockProductBuilder.Setup(p => p.IfA()).Returns(_mockProductBuilder.Object);
            _mockProductBuilder.Setup(p => p.IfB()).Returns(_mockProductBuilder.Object);
            _mockProductBuilder.Setup(p => p.IfC()).Returns(_mockProductBuilder.Object);
            _mockProductBuilder.Setup(p => p.BuildPayment()).Returns(56.15m); 

            var (statusCode, loan) = await _transaction.GetUserByIdAsync(loanID);

            statusCode.Should().Be((int)HttpStatusCode.OK);
            loan.Should().NotBeNull();
            loan!.RepaymentsFrom.Should().Be(56.15m); 
            loan.ID.Should().Be(mockLoan.ID); 
            loan.FirstName.Should().Be(mockLoan.FirstName);

    }


    [Test]
    public async Task Retrieve_Loan_Using_The_ID_Loan_Should_Return_NotFound()
    {
        var loanID = "I_DONT_EXIST";

         _mockQueryDapper.Setup(q => q.GetByIDAsync(loanID)).ReturnsAsync((MoneyMeyLoan?)null);

         var (statusCode, loan) = await _transaction.GetUserByIdAsync(loanID);

        statusCode.Should().Be((int)HttpStatusCode.NotFound);
         loan.Should().BeNull();
    }


    
}
