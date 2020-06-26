using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Prometheus;
using System;
using System.Threading.Tasks;

namespace IdentityService
{
    public class RequestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RequestMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<RequestMiddleware>();
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var path = httpContext.Request.Path.Value;
            var method = httpContext.Request.Method;

            var counter = Metrics.CreateCounter("identity_requests", "HTTP Requests Total", new CounterConfiguration
            {
                LabelNames = new[] { "method", "status" }
            });

            var statusCode = 200;

            try
            {
                await _next.Invoke(httpContext);
            }
            catch (Exception)
            {
                statusCode = 500;
                counter.Labels(method, statusCode.ToString()).Inc();

                throw;
            }

            if (path != "/metrics" && !path.StartsWith("/swagger"))
            {
                statusCode = httpContext.Response.StatusCode;
                counter.Labels(method, statusCode.ToString()).Inc();
            }
        }
    }

    public static class RequestMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestMiddleware>();
        }
    }
}
