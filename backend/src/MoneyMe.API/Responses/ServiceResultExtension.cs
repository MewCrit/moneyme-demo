

namespace MoneyMe.Contracts.Response;

public static class ServiceResultExtension 
{
    public static IResult ServiceResponse(this IResultExtensions result, MoneyMeBaseResponse response)
                     => new CustomResponse(response);

}