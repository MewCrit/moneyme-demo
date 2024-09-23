

namespace MoneyMe.Contracts.Request;

public record ClientLoanRequest
{
    public decimal AmountRequired { get; init; }
    public string? Term { get; init; }

    public string? Title { get; init; }

    public string? FirstName { get; init; }

    public string? LastName { get; init; }
    
    public DateTime? DateOfBirth { get; init; }

    public string? PhoneNumber { get; init; }

    public string? Email { get; init; }

    public string? Product { get; set; }
}
