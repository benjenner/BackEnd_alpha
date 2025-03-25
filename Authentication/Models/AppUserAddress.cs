using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authentication.Models
{
    public class AppUserAddress
    {
        // *Key* definierar UserId som primärnyckel.
        // UserId kommer kan tex vara qwerty123, beroende på det id som guid genererar åt AppUser.
        // "[ForeignKey(nameof(User))]" anger att fältet (UserId) är den främmande nyckel som koopplar
        // AppUserAddress till en specifik AppUser.

        [Key, ForeignKey(nameof(User))]
        public string UserId { get; set; } = null!;

        public AppUser User { get; set; } = null!;

        public string? StreetName { get; set; }

        public string? PostalCode { get; set; }

        public string? City { get; set; }
    }
}