using Microsoft.AspNetCore.Mvc.Filters;
using PasswordManager.API.Models;

namespace PasswordManager.API.Filters;

public class FluentValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            var modelStateErrors = context.ModelState.Where(x => x.Value!.Errors.Count > 0)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value!.Errors.Select(x => x.ErrorMessage)).ToArray();

            var errorResponse = new ErrorResponse();

            foreach (var error in modelStateErrors)
            {
                foreach (var subError in error.Value)
                {
                    var errorModel = new ErrorModel
                    {
                        FieldName = error.Key,
                        Message = subError
                    };

                    errorResponse.Errors.Add(errorModel);
                }
            }

            context.Result = new BadRequestObjectResult(errorResponse);
            return;
        }

        await next();
    }
}
