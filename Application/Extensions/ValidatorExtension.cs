using FluentValidation;

namespace Application.Extensions;

public static class ValidatorExtension
{
    public static IRuleBuilderOptions<T, TProperty> CheckIsCurrentUserIdAsync<T, TProperty>(this IRuleBuilderOptions<T, TProperty> ruleBuilder,
        string? userId)
    {
        return ruleBuilder.Must((idToCheck) =>
        {
            if (idToCheck is not string)
                return false;

            if (userId == null)
                return false;
            
            return idToCheck.ToString() == userId;
        });
    }
    
    public static IRuleBuilderOptions<T, TProperty> CheckIsCurrentUserIdAsync<T, TProperty>(this IRuleBuilderInitial<T, TProperty> ruleBuilder,
        string? userId)
    {
        return ruleBuilder.Must((idToCheck) =>
        {
            if (idToCheck is not string)
                return false;

            if (userId == null)
                return false;
            
            return idToCheck.ToString() == userId;
        });
    }
}