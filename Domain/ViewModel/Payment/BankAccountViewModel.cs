namespace Domain.ViewModel.Payment;

public class BankAccountViewModel
{
    public string? Id { get; set; }
    public required string? Name { get; set; }
    public string? AccountNumber { get; set; }
    public string? Iban { get; set; }
}
