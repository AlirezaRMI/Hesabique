using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;
using Domain.Entities.Trade;
using Domain.Enum.Transeation;

namespace Domain.Entities;

public class Transaction : BaseEntity
{
    public string? Description { get; set; }

    public TransactionStatus Status { get; set; }
    [Required(ErrorMessage = "مبلغ تراکنش نمیتواند خالی باشد")]
    public long Price { get; set; }
    public bool IsConfirmed { get; set; } 
    public DateOnly CreateDate { get; set; }
    public TimeOnly CreatTime { get; set; }

    #region Relation
    
    public string? TenantId { get; set; }
    public Tenant.Tenant Tenant { get; set; }
    
    public string? UserId { get; set; }
    public User User { get; set; }
    
    public string? PaymentId { get; set; }
    public Payment? Payment { get; set; }
    public string? BankAccountId { get; set; }
    public BankAccount? BankAccount { get; set; }
    public string? ChequeId { get; set; }
    public Cheque? Cheque { get; set; }
    [EnumDataType(typeof(TransactionType), ErrorMessage = "نوع تراکنش معتبر نیست")]
    public TransactionType Type { get; set; }

    #endregion
}