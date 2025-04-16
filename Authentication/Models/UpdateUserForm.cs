using Microsoft.AspNetCore.Http;

namespace Authentication.Models
{
    public class UpdateUserForm
    {
        public string UserId { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public IFormFile? NewImage { get; set; }

        public string Email { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string StreetName { get; set; } = null!;

        public string PostalCode { get; set; } = null!;

        public string City { get; set; } = null!;

        public string Role { get; set; } = null!;

        public string JobTitle { get; set; } = null!;
    }
}