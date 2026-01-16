namespace FinFlow.Core.Enums
{
    // Types of transactions in the financial system
    public enum TransactionType
    {
        // Money coming in (salary, sales, client payment)
        // Increases account balance
        Income = 0,
        // Money going out (rent, purchase, bills)
        // Decreases account balance
        Expense = 1,
        // Money moving between user's own accounts
        // Once account increases, another decreases
        // Networth stays the same
        Transfer = 2
    }
}