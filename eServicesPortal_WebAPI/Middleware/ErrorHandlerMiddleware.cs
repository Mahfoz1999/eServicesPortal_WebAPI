using eServicesPortal_Commneds.Exceptions;
using System.Net;
using System.Text.Json;

namespace eServicesPortal_WebAPI.Middleware;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            string errorMessage = error.Message;
            switch (error)
            {
                case ValidException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case NotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case UnauthorizedException:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                default:
                    // unhandled error
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorMessage = "Internal Server Error";
                    break;
            }
            Console.WriteLine(error);
            var result = JsonSerializer.Serialize(errorMessage);
            await response.WriteAsync(result);
        }
    }
}
