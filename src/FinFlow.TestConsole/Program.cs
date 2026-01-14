using System;
using FinFlow.Core.Models;

namespace FinFlow.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== FinFlow Account Test ===\n");

            // Create a new Account
            var checkingAccount = new Account
            {
                Name = "My Checking Account",
                AccountType = "Checking",
                Currency = "USD"
            };

            Console.WriteLine($"Created Account: {checkingAccount.Name}");
            Console.WriteLine($"ID: {checkingAccount.Id}");
            Console.WriteLine($"Initial Balance: {checkingAccount.Currency} {checkingAccount.Balance}");
            Console.WriteLine($"Created At: {checkingAccount.CreatedAt}");
            Console.WriteLine($"Is Valid: {checkingAccount.IsValid()}\n");

            // Deposit money
            Console.WriteLine("--- Depositing $1,000 ---");
            checkingAccount.Deposit(1000.00m);
            Console.WriteLine($"New Balance: {checkingAccount.Currency} {checkingAccount.Balance}\n");

            // Withdraw money
            Console.WriteLine("--- Withdrawing $250 ---");
            checkingAccount.Withdraw(250.00m);
            Console.WriteLine($"New Balance: {checkingAccount.Currency} {checkingAccount.Balance}\n");

            // Try to withdraw too much to see it throw exception
            try
            {
                Console.WriteLine("--- Attempting to withdraw $2,000 (should fail) ---");
                checkingAccount.Withdraw(2000.00m);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Error: {ex.Message}\n");
            }

            // Test invalid account
            var invalidAccount = new Account
            {
               Name = "AB", // Too Short
               AccountType = "Checking",
               Currency = "US" // Wrong Length
            };

            Console.WriteLine($"Invalid Account Name: {invalidAccount.Name}");
            Console.WriteLine($"Is Valid: {invalidAccount.IsValid()}");

            Console.WriteLine("\n=== Test Complete ===");
        }
    }
}