using Domain.Models;
using Swashbuckle.AspNetCore.Filters;

namespace WebApi.Documentation.AppUserEndpoint
{
    public class UserNotFoundExample : IExamplesProvider<ErrorMessage>
    {
        public ErrorMessage GetExamples() => new()
        {
            Message = "User with the given ID was not found."
        };
    }
}