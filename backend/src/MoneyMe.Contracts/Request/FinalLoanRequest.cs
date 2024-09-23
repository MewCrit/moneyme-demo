namespace MoneyMe.Contracts.Request;

public record FinalLoanRequest
{
    public decimal LoanAmount { get; set; }    

    public int? Term { get; set; }  

    public string? Title { get; set; }    

    public string? FirstName { get; set; }    

    public string? LastName { get; set; }       

    public DateTime? DateOfBirth { get; set; }  

    public string? PhoneNumber { get; set; }    

    public string? Email { get; set; }          

    public string? Product { get; set; }     

    public decimal? Repayment { get; set; }    
}
