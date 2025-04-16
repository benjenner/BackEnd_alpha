using Authentication.Models;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Entities
{
    public class AppUserEntity : IdentityUser
    {
        // Id genereras automatiskt av guid (textsträng istället för int)

        [ProtectedPersonalData]
        public string FirstName { get; set; } = null!;

        [ProtectedPersonalData]
        public string LastName { get; set; } = null!;

        public string? JobTitle { get; set; }

        // Kommer alltid vara tom i databas, värde tilldelas när modell skapas upp
        public string? RoleName { get; set; }

        public string? Image { get; set; }

        public AppUserAddressEntity? Address { get; set; }
    }
}