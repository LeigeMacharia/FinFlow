namespace FinFlow.Core.Enums
{
    // Lifecycle status of a transaction
    // Critical for payment processing and reconcilliation
    public enum TransactionStatus
    {
        // Transaction is waiting confirmation
        // Balance not yet updated
        Pending = 0,
        // Transaction completed successfully
        // Balance updated
        Completed = 1,
        // Transaction failed (Payment declined, Insufficient funds)
        // Balance not updated
        Failed = 2,
        // Transaction was cancelled before completion
        // Balance not updated
        Cancelled = 3,
        // Transaction was reversed (refund, chargeback)
        // Creates a new offsetting transaction
        // Original transaction remains in history
        Reversed = 4
    }
}