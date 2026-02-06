using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CitasMedicas.API.Filters;

public sealed class FluentValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        foreach (var (key, value) in context.ActionArguments)
        {
            if (value is null)
                continue;

            var type = value.GetType();
            if (type.IsPrimitive || type == typeof(string))
                continue;

            var validatorType = typeof(IValidator<>).MakeGenericType(type);
            var validator = context.HttpContext.RequestServices.GetService(validatorType) as IValidator;

            if (validator is null)
                continue;

            var validationContext = new ValidationContext<object>(value);
            var result = await validator.ValidateAsync(validationContext);

            if (!result.IsValid)
            {
                context.Result = new BadRequestObjectResult(new ValidationProblemDetails(
                    result.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray()
                        )
                ));

                return;
            }
        }

        await next();
    }
}