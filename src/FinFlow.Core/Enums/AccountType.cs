namespace FinFlow.Core.Enums
{
    // Types of financial accounts supported by FinFlow

    public enum AccountType
    {
        // Standard checking/current account for daily transactions
        Checking = 0,
        // Savings account for storing money
        Savings = 1,
        // Credit card account (Balance is debt)
        CreditCard = 2,
        // Physical cash on hand
        Cash = 3,
        // Investment account (Stocks, Bonds, Crypto)
        // Future feature
        Investment = 4,
        // Loan account (Mortgage, Car loan, personal loan)
        // Future feature
        Loan = 5
    }
}