using Agenda.Exceptions;
using System.Net;
using System.Text.Json;

namespace Agenda.Middleware
{
    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                try
                {
                    await next.Invoke();
                }
                catch (Exception ex)
                {
                    context.Response.ContentType = "application/json";

                    var statusCode = ex switch
                    {
                        BadRequestException => (int)HttpStatusCode.BadRequest,
                        NotFoundException => (int)HttpStatusCode.NotFound,
                        MensajePersonalizadoException => (int)HttpStatusCode.BadRequest,
                        _ => (int)HttpStatusCode.InternalServerError
                    };

                    context.Response.StatusCode = statusCode;

                    var response = new
                    {
                        status = statusCode,
                        error = ex.Message
                    };

                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                }
            });

            return app;
        }
    }
}
