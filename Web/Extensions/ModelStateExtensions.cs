using Application.Common.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Web.Extensions;

public static class ModelStateExtensions
{
    public static Result ToResult(this ModelStateDictionary modelState)
    {
        return modelState.IsValid ? Result.Success() : Result.Failure(modelState.GetErrors());
    }
    
    public static string[] GetErrors(this ModelStateDictionary modelState)
    {
        return modelState.Values.SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToArray();
    }
}