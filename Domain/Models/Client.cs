namespace Domain.Models;

// Följande modell ska skickas tillbaka till klient när en client efterfrågas
public class Client
{
    public int Id { get; set; }
    public string? ClientName { get; set; }
    public DateTime? Created { get; set; }
    public DateTime? Modified { get; set; }
    public bool? IsActive { get; set; }
    public ClientContactInformation ClientContactInformation { get; set; } = new();
    public ClientAddress ClientAddress { get; set; } = new();
}

public class ClientContactInformation
{
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Reference { get; set; }
}

public class ClientAddress
{
    public string BillingAddress { get; set; }
    public string? PostalCode { get; set; }
    public string? City { get; set; }
}

// En override på GetAllAsync
// Här inkluderar vi det som ska hämtas. Sedan används select. Vill tex inte ha id på ContactInformation

// Baserat på dom properties som ska hämtas ut skapas en client-modell.