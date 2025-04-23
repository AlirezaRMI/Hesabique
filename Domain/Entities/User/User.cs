using System.ComponentModel.DataAnnotations;
using Domain.Entities.Common;
using Domain.Entities.Trade;
using Domain.Enum.User;

namespace Domain.Entities;

public class User : BaseEntity
{
    [MaxLength(length:50,ErrorMessage = "نام کاربری نمینواند بیش از 50 کاراکتر باشد")]
    public string? UserName { get; set; }
    
    public string? AccountCode { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; }
    public string? Password { get; set; }
    public Status Status { get; set; }

    #region Relations
    public ICollection<UserRoles>? UserRoles { get; set; }
    public ICollection<Transaction>? Transactions { get; set; }
    public string? TenantId { get; set; }
    public Tenant.Tenant Tenant { get; set; } = null!; 
    public ICollection<JournalEntry> JournalEntries { get; set; } = [];
    public ICollection<Invoice> Invoices { get; set; } = [];
    public ICollection<Payment> Payments { get; set; } = [];
    #endregion
}