namespace Domain.ViewModel.Ledger;

public class AccountTreeNodeViewModel()
{
    public string? Id { get; set; }
    public string AccountCode { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }

    public List<AccountTreeNodeViewModel> Children { get; set; } = new();
}