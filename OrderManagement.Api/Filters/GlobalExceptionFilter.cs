using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OrderManagement.Shared.Exceptions;
using System.Diagnostics;

namespace OrderManagement.Api.Filters;

public class GlobalExceptionFilter(IWebHostEnvironment env, ILogger<GlobalExceptionFilter> logger) : IExceptionFilter
{
    private readonly IWebHostEnvironment env = env;
    private readonly ILogger<GlobalExceptionFilter> logger = logger;

    public void OnException(ExceptionContext context)
    {
        var result = context.Exception switch
        {
            INotAuthorizedException => DomainExceptionHandler(context, StatusCodes.Status401Unauthorized),
            INotFoundException => DomainExceptionHandler(context, StatusCodes.Status404NotFound),
            DomainException => DomainExceptionHandler(context, StatusCodes.Status400BadRequest),
            _ => InternalServerErrorExceptionHandler(context)
        };

        if (result != null)
        {
            context.Result = result;
            context.ExceptionHandled = true;
        }
    }

    private ObjectResult DomainExceptionHandler(ExceptionContext context, int statusCode)
    {
        var ex = (DomainException)context.Exception;
        var problemDetails = CreateProblemDetails(context, env, ex.Key);
        problemDetails.Status = statusCode;

        var errors = ex.Errors
            .GroupBy(e => e.Key, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(e => e.Key, e => e.First().Message);

        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation(ex, "[DomainException] {Title}: {Errors}", problemDetails.Title, errors);

        if (errors.Count > 0)
            problemDetails.Extensions["errors"] = errors;

        if (ex.Data.Count > 0)
            problemDetails.Extensions["data"] = context.Exception.Data;

        return new ObjectResult(problemDetails);
    }

    private ObjectResult InternalServerErrorExceptionHandler(ExceptionContext context)
    {
        var problemDetails = CreateProblemDetails(context, env);
        problemDetails.Status = StatusCodes.Status500InternalServerError;

        logger.LogError(context.Exception,
            "[Exception] {Title}: {Message} - TraceId: {TraceId}",
            problemDetails.Title, context.Exception.Message,
            problemDetails.Extensions["traceId"]);

        if (env.IsProduction() || env.IsEnvironment("producao"))
        {
            var id = problemDetails.Extensions["traceId"] +
                DateTimeOffset.UtcNow.ToString("@yyyyMMddHHmmssK");

            problemDetails.Detail = "Erro ao processar a requisição. " +
                $"Contate o suporte e informe o código [{id}]";
        }

        return new ObjectResult(problemDetails);
    }

    private static ProblemDetails CreateProblemDetails(ExceptionContext context,
        IWebHostEnvironment env, string? title = null)
    {
        var problemDetails = new ProblemDetails
        {
            Title = title ?? context.Exception.GetType().Name,
            Instance = context.HttpContext.Request.Path.Value,
            Detail = context.Exception.Message
        };

        var traceId = Activity.Current?.Id
            ?? context.HttpContext.TraceIdentifier;

        if (traceId != null)
            problemDetails.Extensions["traceId"] = traceId;

        if (!env.IsProduction() && !env.IsEnvironment("producao"))
            problemDetails.Extensions["exception"] = context.Exception.ToString();

        return problemDetails;
    }
}