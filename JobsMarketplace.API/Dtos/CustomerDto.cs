namespace JobsMarketplace.API.Dtos;

// Use record for simplicity coz this will only be used for data transfer
public record CustomerDto(int Id, string FirstName, string LastName)
{
    public string? Email { get; set; }  // Optional property for email (for testing)
}