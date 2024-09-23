

namespace MoneyMe.Contracts.Request;

public record CalculationRequest
{
    public int Term { get; init; }

    public string? Product { get; init; }

    public decimal AmountRequired { get; set; }

}
