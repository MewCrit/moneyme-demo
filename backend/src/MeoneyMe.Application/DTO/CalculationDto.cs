
namespace MeoneyMe.Application.DTO;


public record CalculationDto
{
    public decimal Repayment { get; set; } = 00m;

    public CalculationDto(decimal repayment)
    {   
        this.Repayment = repayment;
    }

}
