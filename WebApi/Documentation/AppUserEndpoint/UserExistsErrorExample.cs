using Domain.Models;
using Swashbuckle.AspNetCore.Filters;

namespace WebApi.Documentation.AppUserEndpoint
{
    public class UserExistsErrorExample : IExamplesProvider<ErrorMessage>
    {
        public ErrorMessage GetExamples() => new()
        {
            Message = "User already exists."
        };
    }
}