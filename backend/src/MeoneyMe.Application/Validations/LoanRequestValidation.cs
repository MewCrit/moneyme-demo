using FluentValidation;
using MeoneyMe.Infrastructure;
using MeoneyMe.Infrastructure.Utils;
using MoneyMe.Contracts.Request;
using MoneyMe.Domain.Enums;

namespace MoneyMe.Application.Validations;

public class LoanRequestValidation : AbstractValidator<ClientLoanRequest>
{
    private readonly IBlacklistQuery _blackListQuery;

    public LoanRequestValidation(IBlacklistQuery blacklist)
    {

        _blackListQuery = blacklist ?? throw new ArgumentNullException(nameof(blacklist));

        RuleFor( x => x.FirstName)
                .NotEmpty()
                .WithMessage("First name is required");

        RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Last name is required");

        RuleFor(x => x.DateOfBirth)
                .NotEmpty()
                .WithMessage("Date of birth required")
                .Must(Utilities.MustBe18)
                .WithMessage("Client must be 18 years old and above");
       
        RuleFor(x => x.AmountRequired)
                .GreaterThan(0)
                .WithMessage("Amount is required");
       
        RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.")
                .MustAsync(async (email, cancellation) => await ValidateIfContactIsAllowed(email , ContactTypes.EmailDomain))
                .WithMessage("This email domain is not allowed.");

        RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .MinimumLength(12)
                .WithMessage("This phone number length not allowed")
                .MustAsync(async (phone, cancellation) => await ValidateIfContactIsAllowed(phone , ContactTypes.MobileNumber))
                .WithMessage("This phone number is not allowed.");
    }


    private async Task<bool> ValidateIfContactIsAllowed(string value, string type)
    {
        if (type.Equals(ContactTypes.EmailDomain, StringComparison.Ordinal))
        {
            var domainIndex = value.IndexOf("@");
            if (domainIndex != -1)
            {
                var domainName = value[(domainIndex + 1)..];
                return await _blackListQuery.GetBlacklisted(domainName, type); 
            }
        }

        return await _blackListQuery.GetBlacklisted(value, type);
    }



}

