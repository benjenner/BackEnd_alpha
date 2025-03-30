using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

[Index(nameof(ClientName), IsUnique = true)]
public class ClientEntity
{
    [Key]
    public int Id { get; set; }

    public string ClientName { get; set; } = null!;
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public bool IsActive { get; set; }

    // ClientEntity har en ContactInformationEntity samt en ClientAdressEntity.
    // Dessa sätts till virtual för att möjliggöra lazy loading
    public virtual ClientContactInformationEntity ClientContactInformation { get; set; } = null!;

    public virtual ClientAddressEntity Address { get; set; } = null!;

    public virtual ICollection<ProjectEntity> Projects { get; set; }
}

// ClientAddressEntity har clientId och en referens till ClientEntity
// Både en främmane nyckel och primärnyckel.
// En klient kan bara ha en adress
// Det här möjliggör att man inte behöver skapa en adress innan en klient. 