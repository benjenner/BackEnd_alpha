using Authentication.Models;
using Swashbuckle.AspNetCore.Filters;

namespace WebApi.Documentation.AppUserEndpoint
{
    public class UpdateUserFormExample : IExamplesProvider<UpdateUserForm>
    {
        public UpdateUserForm GetExamples() => new()
        {
            UserId = "12345",
            FirstName = "Jane",
            LastName = "Smith",
            NewImage = null,
            Email = "jane.smith@domain.com",
            PhoneNumber = "+987654321",
            StreetName = "456 Elm Street",
            PostalCode = "54321",
            City = "New York",
            Role = "Admin",
            JobTitle = "Project Manager"
        };
    }
}