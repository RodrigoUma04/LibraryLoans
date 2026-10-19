using LibraryLoans.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace LibraryLoans.API.Extensions;

public static class ResultExtensions
{
    public static ObjectResult ToProblem(this Result result)
    {
        var status = result.ErrorType switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status400BadRequest
        };

        var problem = new ProblemDetails
        {
            Status = status,
            Title = result.ErrorType.ToString(),
            Detail = string.Join("; ", result.Errors),
            Extensions = { ["errors"] = result.Errors }
        };
        
        return new ObjectResult(problem) { StatusCode = status };
    }
}