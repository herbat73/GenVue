using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace GenVue.Middleware
{
    public class ServerSentEventsMiddleware
    {
        private readonly RequestDelegate _next;

        public ServerSentEventsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            if (context.Request.Headers["Accept"] == "text/event-stream")
            {
                context.Response.ContentType = "text/event-stream";
                context.Response.Body.Flush();

                return Task.FromResult(true);
            }
            else
            {
                return _next(context);
            }
        }
    }

    public static class ServerSentEventsMiddlewareExtensions
    {
        public static IApplicationBuilder UseServerSentEventsMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ServerSentEventsMiddleware>();
        }
    }
}
