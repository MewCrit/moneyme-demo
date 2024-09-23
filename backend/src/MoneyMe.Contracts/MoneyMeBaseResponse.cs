
namespace MoneyMe.Contracts;

public record MoneyMeBaseResponse(
    int StatusCode,
    object? Result
)
{}