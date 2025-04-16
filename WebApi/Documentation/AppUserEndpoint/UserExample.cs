using Authentication.Models;
using Swashbuckle.AspNetCore.Filters;

namespace WebApi.Documentation.AppUserEndpoint
{
    public class UserExample : IExamplesProvider<AppUser>
    {
        public AppUser GetExamples() => new()
        {
            Id = "ae5f645a-9537-40c0-9016-2fffe881b1b3",
            Image = "u_ab3214b0-14b5-4f23-a8db-6466f465ce6d.png",
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@domain.com",
            PhoneNumber = "070619687",
            Address = "Alphyddevägen 1",
            PostalCode = "12345",
            City = "Stockholm",
        };
    }
}