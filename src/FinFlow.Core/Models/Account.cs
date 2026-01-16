using System;
using FinFlow.Core.Enums;

namespace FinFlow.Core.Models
{
    public class Account
    {
        // Primary key - uniquely identifies each account
        public Guid Id {get; set; }

        // Account name ("My Checking Account", "Emergency Savings")
        // Nullable because it will be set after construction (from API request)
        public string? Name {get; set; }

        // Type of account - nullable for the same reason
        public AccountType AccountType {get; set; }

        // Current balance in the account
        public decimal Balance {get; set; }

        // Currency code (USD, EUR, KES)
        public string Currency {get; set; }

        // When the account was created
        public DateTime CreatedAt {get; set; }

        // When the account was last updated
        public DateTime UpdatedAt {get; set; }

        // Is the account active or closed?
        public bool IsActive {get; set; }

        // Constructor - runs when you create a new account
        public Account()
        {
            Id = Guid.NewGuid(); // Generate unique ID
            CreatedAt = DateTime.UtcNow; // Set creation time
            UpdatedAt = DateTime.UtcNow; // Set update time
            IsActive = true; // New accounts are active by default
            Balance = 0; // Start with zero balance
            Currency = "USD"; // Default to USD
            AccountType = AccountType.Checking;
        }

        // Validates that the account has all required data
         public bool IsValid()
        {
            // Name is required and can't be empty
            if (string.IsNullOrWhiteSpace(Name))
            return false;
            // Name must be reasonable length (3 - 100 characters)
            if (Name.Length < 3 || Name.Length > 100)
            return false;
            // Currency is required
            if (string.IsNullOrWhiteSpace(Currency))
            return false;
            // Currency must be 3 characters (ISO 4217 standard: USD, EUR, KES)
            if (Currency.Length != 3)
            return false;
            // All checks passed
            return true;
        }

        // Updates the balance and timestamp
        public void UpdateBalance(decimal newBalance)
        {
            Balance = newBalance;
            UpdatedAt = DateTime.UtcNow;
        }

        // Deposits money into the account
        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Deposit amount must be positive", nameof(amount));

            Balance += amount;
            UpdatedAt = DateTime.UtcNow;
        }

        // Withdraws money from the account
        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Withdrawal amount must be positive", nameof(amount));
            
            if (amount > Balance)
                throw new InvalidOperationException("Insufficient funds");
            
            Balance -= amount;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}