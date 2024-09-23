
using System.Net;
using System.Text.Json;

namespace MoneyMe.Contracts.Response;

public class ExceptionHandling
{
    private readonly RequestDelegate _next;
    
    public ExceptionHandling(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

   private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        if (exception is FluentValidation.ValidationException validationException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            
            var validationErrors = validationException.Errors
                        .GroupBy(error => error.PropertyName)
                        .Select(group => group.First())
                        .Select(error => new 
                        {
                            Field = error.PropertyName,    
                            Error = error.ErrorMessage     
                        })
                        .ToList(); 
                        
            var errorResult = JsonSerializer.Serialize(new
            {
                statusCode = context.Response.StatusCode,
                message = "Validation failed.",
                errors = validationErrors
            });

            return context.Response.WriteAsync(errorResult);
        }

        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        Console.WriteLine($"Something went wrong: {exception}");
        var result = JsonSerializer.Serialize(new
        {
            statusCode = context.Response.StatusCode,
            message = "Internal Server Error. Please try again later.",
            detailed = exception.Message
        });

        return context.Response.WriteAsync(result);
    }
}
