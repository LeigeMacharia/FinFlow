/* using System;
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
} */

using System;
using FinFlow.Core.Models;
using FinFlow.Core.Enums;

namespace FinFlow.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== FinFlow Models Test ===\n");

            // Test 1: Create Account with Enum
            Console.WriteLine("--- Test 1: Account with Enum ---");
            var checkingAccount = new Account
            {
                Name = "My Checking Account",
                AccountType = AccountType.Checking, // Using enum!
                Currency = "USD"
            };
            
            Console.WriteLine($"Account: {checkingAccount.Name}");
            Console.WriteLine($"Type: {checkingAccount.AccountType}"); // Prints "Checking"
            Console.WriteLine($"Balance: {checkingAccount.Currency} {checkingAccount.Balance}");
            Console.WriteLine($"Is Valid: {checkingAccount.IsValid()}\n");

            // Test 2: Create Category
            Console.WriteLine("--- Test 2: Category ---");
            var groceriesCategory = new Category
            {
                Name = "Groceries",
                Type = "expense",
                Description = "Food and household items"
            };
            
            Console.WriteLine($"Category: {groceriesCategory.Name}");
            Console.WriteLine($"Type: {groceriesCategory.Type}");
            Console.WriteLine($"Is Valid: {groceriesCategory.IsValid()}\n");

            // Test 3: Create Income Transaction
            Console.WriteLine("--- Test 3: Income Transaction ---");
            var salaryTransaction = new Transaction
            {
                Amount = 5000.00m,
                Currency = "USD",
                Type = TransactionType.Income, // Using enum!
                Description = "January Salary",
                AccountId = checkingAccount.Id,
                CategoryId = groceriesCategory.Id,
                TransactionDate = DateTime.UtcNow
            };
            
            Console.WriteLine($"Transaction: {salaryTransaction.Description}");
            Console.WriteLine($"Amount: {salaryTransaction.Currency} {salaryTransaction.Amount}");
            Console.WriteLine($"Type: {salaryTransaction.Type}"); // Prints "Income"
            Console.WriteLine($"Status: {salaryTransaction.Status}"); // Prints "Pending"
            Console.WriteLine($"Is Valid: {salaryTransaction.IsValid()}\n");

            // Test 4: Complete Transaction
            Console.WriteLine("--- Test 4: Complete Transaction ---");
            salaryTransaction.Complete();
            Console.WriteLine($"Status after completion: {salaryTransaction.Status}\n");

            // Test 5: Create Expense Transaction
            Console.WriteLine("--- Test 5: Expense Transaction ---");
            var groceryExpense = new Transaction
            {
                Amount = 150.50m,
                Currency = "USD",
                Type = TransactionType.Expense,
                Description = "Weekly groceries",
                AccountId = checkingAccount.Id,
                CategoryId = groceriesCategory.Id,
                PaymentMethod = "Credit Card"
            };
            
            groceryExpense.Complete();
            
            Console.WriteLine($"Expense: {groceryExpense.Description}");
            Console.WriteLine($"Amount: {groceryExpense.Currency} {groceryExpense.Amount}");
            Console.WriteLine($"Type: {groceryExpense.Type}");
            Console.WriteLine($"Payment Method: {groceryExpense.PaymentMethod}");
            Console.WriteLine($"Status: {groceryExpense.Status}\n");

            // Test 6: Calculate Balance Impact
            Console.WriteLine("--- Test 6: Balance Impact ---");
            var incomeImpact = salaryTransaction.GetBalanceImpact(checkingAccount.Id);
            var expenseImpact = groceryExpense.GetBalanceImpact(checkingAccount.Id);
            
            Console.WriteLine($"Income impact on balance: {incomeImpact:C}");
            Console.WriteLine($"Expense impact on balance: {expenseImpact:C}");
            Console.WriteLine($"Net change: {(incomeImpact + expenseImpact):C}\n");

            // Test 7: Create Transfer
            Console.WriteLine("--- Test 7: Transfer Transaction ---");
            var savingsAccount = new Account
            {
                Name = "Savings Account",
                AccountType = AccountType.Savings,
                Currency = "USD"
            };
            
            var transferTransaction = new Transaction
            {
                Amount = 1000.00m,
                Currency = "USD",
                Type = TransactionType.Transfer,
                Description = "Moving to savings",
                AccountId = checkingAccount.Id, // From checking
                ToAccountId = savingsAccount.Id, // To savings
                CategoryId = groceriesCategory.Id
            };
            
            Console.WriteLine($"Transfer: {transferTransaction.Description}");
            Console.WriteLine($"From Account: {checkingAccount.Name}");
            Console.WriteLine($"To Account: {savingsAccount.Name}");
            Console.WriteLine($"Amount: {transferTransaction.Currency} {transferTransaction.Amount}");
            Console.WriteLine($"Is Valid: {transferTransaction.IsValid()}\n");

            // Test 8: Create Reversal
            Console.WriteLine("--- Test 8: Transaction Reversal ---");
            var reversalTransaction = groceryExpense.CreateReversal();
            
            Console.WriteLine($"Original: {groceryExpense.Description}");
            Console.WriteLine($"Reversal: {reversalTransaction.Description}");
            Console.WriteLine($"Reversal Reference: {reversalTransaction.ExternalReference}\n");

            // Test 9: Soft Delete
            Console.WriteLine("--- Test 9: Soft Delete ---");
            Console.WriteLine($"Before delete - IsDeleted: {transferTransaction.IsDeleted}");
            transferTransaction.Delete();
            Console.WriteLine($"After delete - IsDeleted: {transferTransaction.IsDeleted}");
            Console.WriteLine($"Deleted at: {transferTransaction.DeletedAt}\n");

            // Test 10: Transaction Status Lifecycle
            Console.WriteLine("--- Test 10: Status Lifecycle ---");
            var testTransaction = new Transaction
            {
                Amount = 50.00m,
                Currency = "USD",
                Type = TransactionType.Expense,
                AccountId = checkingAccount.Id,
                CategoryId = groceriesCategory.Id
            };
            
            Console.WriteLine($"Initial status: {testTransaction.Status}");
            
            try
            {
                testTransaction.Cancel();
                Console.WriteLine($"After cancel: {testTransaction.Status}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\n=== All Tests Complete! ===");
        }
    }
}