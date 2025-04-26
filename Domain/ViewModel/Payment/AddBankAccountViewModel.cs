namespace Domain.ViewModel.Payment;

public class AddBankAccountViewModel
{
    public string? Name { get; set; }
    public required string AccountNumber { get; set; }
    public string? Iban { get; set; }
    public string? TenantId { get; set; }
}
