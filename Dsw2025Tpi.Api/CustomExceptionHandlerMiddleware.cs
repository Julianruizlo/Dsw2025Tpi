using Dsw2025Tpi.Application.Exceptions;
using System.Net;
using System.Text.Json;
using ApplicationException = Dsw2025Tpi.Application.Exceptions.ApplicationException;

public class CustomExceptionHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            context.Response.ContentType = "application/json";

            var statusCode = e switch
            {
                EntityNotFoundException => HttpStatusCode.NotFound,
                NoContentException => HttpStatusCode.NoContent,
                DuplicatedEntityException => HttpStatusCode.BadRequest,
                BadRequestException => HttpStatusCode.BadRequest,
                ApplicationException => HttpStatusCode.BadRequest,
                ArgumentException => HttpStatusCode.BadRequest,
                InvalidOperationException => HttpStatusCode.BadRequest,
                UnauthorizedException => HttpStatusCode.Unauthorized,
                _ => HttpStatusCode.InternalServerError
                
            };

            context.Response.StatusCode = (int)statusCode;

            var errorResponse = new
            {
                status = (int)statusCode,
                title = statusCode.ToString(),
                detail = e.Message
            };

            var json = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(json);
        }
    }
}




















































/*using Dsw2025Tpi.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace Dsw2025Tpi.Api
{
    public class CustomExceptionHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (EntityNotFoundException enf)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                context.Response.ContentType = "application/json";

                var problem = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.NotFound,
                    Title = "Not Found",
                    Detail = ""
                };

                var json = JsonSerializer.Serialize(problem);
                await context.Response.WriteAsync(json);
            }
            catch (Exception e)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var problem = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Title = "Server error",
                    Detail = "An internal server error has occurred"
                };

                var json = JsonSerializer.Serialize(problem);
                await context.Response.WriteAsync(json);
            }
            
        }
    }
}
*/
