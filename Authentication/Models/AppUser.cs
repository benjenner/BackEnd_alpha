using Microsoft.AspNetCore.Identity;

namespace Authentication.Models
{
    // Ärver från Identity som innehåller properties som Username, Password osv.
    public class AppUser : IdentityUser
    {
        // Id genereras automatiskt av guid (textsträng istället för int)

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? JobTitle { get; set; }

        public AppUserAddress? Address { get; set; }
    }
}