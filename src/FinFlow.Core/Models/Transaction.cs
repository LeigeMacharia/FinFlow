using System;
using FinFlow.Core.Enums;

namespace FinFlow.Core.Models
{
    // Represents a financial transaction (Income, Expense or Tranfer)
    // Core of the FInancial Tracking System
    public class Transaction
    {
        // Unique identifier
        public Guid Id { get; set; }
        // Transaction amount - always positive, type determines direction
        // Use decimal, never float or double
        public decimal Amount { get; set; }
        // Currency of the transcation
        public string Currency { get; set; }
        // Type: Income, Expense, or Transfer
        public TransactionType Type { get; set; }
        // Current status; Pending,Completed, Failed
        public TransactionStatus Status { get; set; }
        // Description (-January Rent, Coffee at Java)
        public string? Description { get; set; }
        // Date when transaction actually occurred (not when recorded)
        public DateTime TransactionDate { get; set; }
        // Category
        public Guid CategoryId { get; set; }
        // Navigation property to Category - EF Core
        public Category? Category { get; set; }
        // For Income/Expense: THe account affected
        // FOr Transfer: The source account - money leaving
        public Guid AccountId { get; set; }
        // Navigation property to Amount
        public Account? Account { get; set; }
        // For Transfers only: Destination account - money arriving
        public Guid? ToAccountId { get; set; }
        // Navigation property to destination account
        public Account? ToAccount { get; set; }
        // Optional: Reference to related invoice
        public Guid? InvoiceId { get; set; }
        // Optional: Payment method used- Cash, Credit Card, Bank Transfer
        public string? PaymentMethod { get; set;}
        // Optional: External reference (Stripe payment ID, check number)
        public string? ExternalReference { get; set; }
        // Optional: Notes or additional details
        public string? Notes { get; set; }
        // Optional: Attach receipt image or document
        public string? ReceiptUrl { get; set; }
        // Tags for flexible categorization - business, tax-deductible
        public string? Tags { get; set; }
        // Audit trail
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        // Soft delete - never actually delete transactions(compliance)
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        // Constructor
        public Transaction()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            TransactionDate = DateTime.UtcNow;
            Status = TransactionStatus.Pending; // Start as pending
            Currency = "USD";
            IsDeleted = false;
        } 

        // Vallidates transaction has all required data
        public bool IsValid()
        {
            // Amount must be positive
            if (Amount <= 0)
                return false;
            // Currency required
            if (string.IsNullOrWhiteSpace(Currency) || Currency.Length != 3)
                return false;
            // Category required
            if (CategoryId == Guid.Empty)
                return false;
            // For transfers, ToAccountId is required
            if (Type == TransactionType.Transfer && ToAccountId == null)
                return false;
            // Transfer can't be to the same account
            if (Type == TransactionType.Transfer && ToAccountId == AccountId)
                return false;
            
            return true;
        }

        // Marks transaction ascompleted and updates timestamp
        public void Complete()
        {
            Status = TransactionStatus.Completed;
            UpdatedAt = DateTime.UtcNow;
        }

        // Marks transactionas failed with reason
        public void Fail()
        {
            Status = TransactionStatus.Failed;
            UpdatedAt = DateTime.UtcNow;
        }

        // Cancels a pending transaction
        public void Cancel()
        {
            if (Status != TransactionStatus.Pending)
                throw new InvalidOperationException("Only pending transactions can be cancelled");

            Status = TransactionStatus.Cancelled;
            UpdatedAt = DateTime.UtcNow;
        }

        // Soft Delete - Marks as deleted but don't actually remove from database
        // Critical for audit trail and compliance
        public void Delete()
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        // Creates a reversal transaction - for refunds, corrections
        public Transaction CreateReversal()
        {
            return new Transaction
            {
                Amount = this.Amount,
                Currency = this.Currency,
                Type = this.Type,
                AccountId = this.AccountId,
                ToAccountId = this.ToAccountId,
                CategoryId = this.CategoryId,
                Description = $"Reversal: {this.Description}",
                TransactionDate = DateTime.UtcNow,
                Status = TransactionStatus.Completed,
                ExternalReference = $"REVERSAL_{this.Id}",
                Notes = $"Reversal of transaction {this.Id}"
            };
        }

        // Calculates the net effect on account balance
        // Income = +Amont, Expense = -Amount, Transfer = depends on account
        public decimal GetBalanceImpact(Guid accountId)
        {
            // For income, balance increases
            if (Type == TransactionType.Income && AccountId == accountId)
                return Amount;
            // For expense, balance decreases
            if (Type == TransactionType.Expense && AccountId == accountId)
                return -Amount;
            // For transfer - if this is the source account, decrease
            if (Type == TransactionType.Transfer && AccountId == accountId)
                return -Amount;
            // For transfer - if this is the destination account, increase
            if (Type == TransactionType.Transfer && ToAccountId == accountId)
                return Amount;
            
            // Transaction doesn't affect this account
            return 0;
        }
    }
}