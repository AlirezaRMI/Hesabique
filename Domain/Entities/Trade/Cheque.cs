using Domain.Entities.Common;

namespace Domain.Entities.Trade;

public class Cheque : BaseEntity
{
    public string BankAccountId { get; set; }
    public BankAccount BankAccount { get; set; } = null!;

    public required string ChequeNo { get; set; }
    public DateTime DueDate { get; set; }
    public decimal Amount { get; set; }
    public bool Cashed { get; set; }

    public Payment? Payment { get; set; }
}