

namespace MeoneyMe.Application.DTO;

public record ClientLoanDTO
{
    public string? ID { get; set; }
    
    public decimal AmountRequired { get; set; }

    public string? Term { get; set; }

    public string? Title { get; set; }     

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? Product { get; set; }

    public decimal RepaymentsFrom { get; set; }

   
}