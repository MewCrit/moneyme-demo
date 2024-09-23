using FluentValidation;
using MeoneyMe.Infrastructure;
using MeoneyMe.Infrastructure.Utils;
using MoneyMe.Contracts.Request;
using MoneyMe.Domain.Enums;

namespace MoneyMe.Application.Validations;

public class FinalLoanValidation : AbstractValidator<FinalLoanRequest>
{
    private readonly IBlacklistQuery _blackListQuery;

    public FinalLoanValidation(IBlacklistQuery blacklist)
    {

        _blackListQuery = blacklist ?? throw new ArgumentNullException(nameof(blacklist));
 
        RuleFor(x => x.DateOfBirth)
                .NotEmpty()
                .WithMessage("Date of birth required")
                .Must(Utilities.MustBe18)
                .WithMessage("Client must be 18 years old and above");
        
       
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

     // TODO : Repeated form Client loan Validation
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

