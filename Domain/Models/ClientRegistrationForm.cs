using Microsoft.AspNetCore.Http;

namespace Domain.Models;

public class ClientRegistrationForm
{
    public string ClientName { get; set; } = null!;
    public string Email { get; set; } = null!;

    public IFormFile? Image { get; set; }
    public string? Phone { get; set; }
    public string? Reference { get; set; }
    public string BillingAddress { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string City { get; set; } = null!;
}