using AutoMapper;
using Domain.Entities.Ledger;
using Domain.Entities;
using Domain.Entities.Trade;
using Domain.ViewModel;
using Domain.ViewModel.Invoice;
using Domain.ViewModel.Ledger;
using Domain.ViewModel.Payment;
using Domain.ViewModel.Transaction;
using Domain.ViewModel.User;

namespace Application.MappingProfiles
{
    public class ProfileMapping : Profile
    {
        public ProfileMapping()
        {
            // Ledger
            CreateMap<Account, AccountViewModel>();
            CreateMap<JournalEntry, JournalEntryViewModel>()
                .ForMember(dest => dest.Lines,
                    opt
                        =>
                        opt.MapFrom(src => src.Lines));
            CreateMap<JournalLine, JournalLineViewModel>();
            CreateMap<AddAccountViewModel, Account>();
            CreateMap<AddJournalEntryViewModel, JournalEntry>();
            CreateMap<EditAccountViewModel, Account>();

            // Invoice
            CreateMap<Invoice, InvoiceViewModel>();
            CreateMap<InvoiceLine, InvoiceLineViewModel>();
            CreateMap<AddInvoiceViewModel, Invoice>();
            CreateMap<AddInvoiceLineViewModel, InvoiceLine>();

            // Payment
            CreateMap<Payment, PaymentViewModel>();
            CreateMap<AddPaymentViewModel, Payment>();
            CreateMap<BankAccount, BankAccountViewModel>();
            CreateMap<AddBankAccountViewModel, BankAccount>();

            // Transaction
            CreateMap<Transaction, TransactionViewModel>()
                .ForMember(dest => dest.IsDelete, opt => opt.Ignore());
            CreateMap<AddTransactionViewModel, Transaction>();
            CreateMap<EditeTransactionViewModel, Transaction>();

            // User
            CreateMap<User, UserViewModel>();
            CreateMap<AddUserViewModel, User>();
            CreateMap<RegisterViewModel, User>();
            CreateMap<EditeUserViewModel, User>();
        }
    }
}