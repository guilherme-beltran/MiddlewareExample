using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Net;
using System.Reflection;
using System.Text.Json;

namespace MiddlewareExample;

internal sealed class GlobalExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
	private readonly IServiceExample _serviceExample;

    public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger, IServiceExample serviceExample)
    {
        _logger = logger;
        _serviceExample = serviceExample;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
		try
		{
			await _serviceExample.DoSomething();

            var controllerName = context.GetRouteValue("controller")?.ToString();

            var actionDescriptor = context.GetEndpoint()?.Metadata.GetMetadata<ControllerActionDescriptor>();
            var actionName = actionDescriptor?.ActionName;

			_logger.LogInformation($"Controller: {controllerName} - Action: {actionName}");

            await next(context);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, ex.Message);

			context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

			ProblemDetails problem = new()
			{
				Status = (int)HttpStatusCode.InternalServerError,
				Type = "Server error",
				Title = "Server error",
				Detail = "An internal server has occurred"
			};

			string json = JsonSerializer.Serialize(problem);

			await context.Response.WriteAsync(json);

			context.Response.ContentType = "application/json";
		}
    }
}
