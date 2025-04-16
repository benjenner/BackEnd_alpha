using Authentication.Models;
using Swashbuckle.AspNetCore.Filters;

namespace WebApi.Documentation.AppUserEndpoint
{
    public class UserRegistrationFormExample : IExamplesProvider<UserRegistrationForm>
    {
        public UserRegistrationForm GetExamples() => new()
        {
            FirstName = "Alice",
            LastName = "Johnson",
            Email = "alice.johnson@example.com",
            Password = "SecurePassword123!"
        };
    }
}