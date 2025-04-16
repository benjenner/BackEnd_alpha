using Authentication.Models;
using Swashbuckle.AspNetCore.Filters;

namespace WebApi.Documentation.AppUserEndpoint
{
    public class SignInExample : IExamplesProvider<SignInForm>
    {
        public SignInForm GetExamples() => new()
        {
            Email = "john.doe@domain.com",
            Password = "BytMig123!"
        };
    }
}