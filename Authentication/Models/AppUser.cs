using Microsoft.AspNetCore.Identity;

namespace Authentication.Models
{
    // Ärver från Identity som innehåller properties som Username, Password osv.
    public class AppUser : IdentityUser
    {
        // Id genereras automatiskt av guid (textsträng istället för int)

        [ProtectedPersonalData]
        public string? FirstName { get; set; }

        [ProtectedPersonalData]
        public string? LastName { get; set; }

        public string? JobTitle { get; set; }

        public AppUserAddress? Address { get; set; }
    }
}