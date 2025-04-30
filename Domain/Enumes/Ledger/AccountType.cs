using System.ComponentModel.DataAnnotations;

namespace Domain.Enumes.Ledger;

public enum AccountType
{
    [Display(Name = "جاری")]
    CurrentAccount,
    [Display(Name = "پس انداز")]
    Savings,
    [Display(Name = "درآمد")]
    Income,
    [Display(Name = "سود")]
    Interest,
    [Display(Name = "سایر")]
    Other
}