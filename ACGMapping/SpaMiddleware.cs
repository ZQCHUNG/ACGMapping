using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ACGMapping
{
    public class SpaMiddleware
    {
        private readonly RequestDelegate _next;

        public SpaMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next.Invoke(context);
            if (context.Response.StatusCode == 404 &&
                !System.IO.Path.HasExtension(context.Request.Path.Value) &&
                !context.Request.Path.Value.StartsWith("/api"))
            {
                context.Request.Path = "~/Index.html";
                context.Response.StatusCode = 200;
            }

            await _next.Invoke(context);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class SpaMiddlewareExtensions
    {
        public static IApplicationBuilder UseSpaMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SpaMiddleware>();
        }
    }
}