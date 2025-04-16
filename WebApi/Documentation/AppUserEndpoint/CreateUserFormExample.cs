using Authentication.Models;
using Swashbuckle.AspNetCore.Filters;

namespace WebApi.Documentation.AppUserEndpoint;

public class CreateUserFormExample : IExamplesProvider<CreateUserForm>
{
    public CreateUserForm GetExamples() => new()
    {
        Email = "example@domain.com",
        FirstName = "John",
        LastName = "Doe",
        NewImage = null, // Exempel kan vara en fil eller null
        JobTitle = "Software Developer",
        PhoneNumber = "+123456789",
        Role = "User",
        Address = "123 Main Street",
        PostalCode = "12345",
        City = "Metropolis"
    };
}