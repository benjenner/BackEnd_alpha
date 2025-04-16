using Authentication.Models;
using Swashbuckle.AspNetCore.Filters;

namespace WebApi.Documentation.AppUserEndpoint
{
    public class UpdateUserExample : IExamplesProvider<UpdateUserForm>
    {
        public UpdateUserForm GetExamples() => new()
        {
            UserId = "ae5f645a-9537-40c0-9016-2fffe881b1b3",
            NewImage = null,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@domain.com",
            PhoneNumber = "0706123873",
            StreetName = "Vägen 2",
            PostalCode = "12345",
            City = "Stockholm",
        };
    }
}