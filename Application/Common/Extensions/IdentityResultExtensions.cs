using Application.Common.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Common.Extensions;

public static class IdentityResultExtensions
{
    public static Result ToAppResult(this IdentityResult identityResult)
    {
        return identityResult.Succeeded ? Result.Success() : Result.Failure(identityResult.GetErrors());
    }
    
    public static string[] GetErrors(this IdentityResult identityResult)
    {
        return identityResult.Errors.Select(e => e.Description).ToArray();
    } 
}