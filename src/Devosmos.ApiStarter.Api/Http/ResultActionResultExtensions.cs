using Devosmos.ApiStarter.Application.Common.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Devosmos.ApiStarter.Api.Http;

public static class ResultActionResultExtensions
{
    public static IActionResult ToActionResult<T>(this Result<T> result, ControllerBase controller)
    {
        if (result.IsSuccess)
        {
            return controller.Ok(result.Value);
        }

        var error = result.Error ?? ApplicationError.Failure("unknown", "An unknown error occurred.");
        var statusCode = MapStatusCode(error.Type);

        if (error.Type == ApplicationErrorType.Validation && error.ValidationErrors is not null)
        {
            var modelState = new ModelStateDictionary();
            foreach (var validationError in error.ValidationErrors)
            {
                foreach (var message in validationError.Value)
                {
                    modelState.AddModelError(validationError.Key, message);
                }
            }

            var validationProblem = new ValidationProblemDetails(modelState)
            {
                Title = "Validation failed.",
                Detail = error.Message,
                Status = statusCode,
                Type = "https://httpstatuses.com/400",
                Instance = controller.HttpContext.Request.Path
            };

            validationProblem.Extensions["code"] = error.Code;
            return new ObjectResult(validationProblem)
            {
                StatusCode = statusCode
            };
        }

        var problem = new ProblemDetails
        {
            Title = error.Code,
            Detail = error.Message,
            Status = statusCode,
            Type = $"https://httpstatuses.com/{statusCode}",
            Instance = controller.HttpContext.Request.Path
        };

        problem.Extensions["code"] = error.Code;

        return new ObjectResult(problem)
        {
            StatusCode = statusCode
        };
    }

    private static int MapStatusCode(ApplicationErrorType type)
    {
        return type switch
        {
            ApplicationErrorType.Validation => StatusCodes.Status400BadRequest,
            ApplicationErrorType.NotFound => StatusCodes.Status404NotFound,
            ApplicationErrorType.Conflict => StatusCodes.Status409Conflict,
            ApplicationErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ApplicationErrorType.Forbidden => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}
