using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Extensions.Attributes;

// Attributet kan användas för att skydda upp controllers OCH metoder med en admin-nyckel

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class UseAdminApiKeyAttribute : Attribute, IAsyncActionFilter
{
    // Hanterar validering av nyckeln innan en åtgärd körs
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
      
        var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        var adminApiKey = configuration["SecretKeys:Admin"];

        // Om nyckeln inte finns med i anropet
        if (!context.HttpContext.Request.Headers.TryGetValue("X-ADM-API-KEY", out var providedApiKey))
        {
            context.Result = new UnauthorizedObjectResult("Invalid api-key or api-key is missing.");
            return;
        }

        // Validerar att den tillhandahålna nyckeln matchar adminKey
        if (string.IsNullOrEmpty(adminApiKey) || !string.Equals(providedApiKey, adminApiKey))
        {
            context.Result = new UnauthorizedObjectResult("Invalid api-key.");
            return;
        }

        // Fortsätt till nästa steg (ActionExecutionDelegate)
        await next();
    }
}