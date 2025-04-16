using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Models
{
    public class CreateUserForm
    {
        public string Email { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public IFormFile? NewImage { get; set; }

        public string JobTitle { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string Role { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string PostalCode { get; set; } = null!;

        public string City { get; set; } = null!;
    }
}