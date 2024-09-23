
using System.Net.Mime;
using MoneyMe.Contracts;

public class CustomResponse : IResult
{
    private MoneyMeBaseResponse _apiResponse;

    public CustomResponse(MoneyMeBaseResponse apiResponse)
        => _apiResponse =  apiResponse ?? throw new ArgumentNullException(nameof(apiResponse));

    public async Task ExecuteAsync(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = _apiResponse.StatusCode;
        httpContext.Response.ContentType = MediaTypeNames.Application.Json;
        await httpContext.Response.WriteAsJsonAsync(_apiResponse, CancellationToken.None);
    }
}
