using Domain.Models;
using Swashbuckle.AspNetCore.Filters;

namespace WebApi.Documentation.AppUserEndpoint
{
    public class SignInErrorExample : IExamplesProvider<ErrorMessage>
    {
        public ErrorMessage GetExamples() => new()
        {
            Message = "Invalid email or password."
        };
    }
}