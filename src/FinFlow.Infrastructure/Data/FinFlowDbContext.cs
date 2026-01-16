using Microsoft.EntityFrameworkCore;
using FinFlow.Core.Models;

namespace FinFlow.Infrastructure.Data
{
    // Database context for FinFlow application
    // Manages database connections and entity tracking
    public class FinFlowDbContext : DbContext
    {
        // Constructor - recieves configuration from dependency injection
        public FinFlowDbContext(DbContextOptions<FinFlowDbContext> options)
            : base(options)
        {
        }

        // Database tables - DbSets
        // Each DbSet reps a table in PostgreSQL
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }

        // Configure model relationships and constraints
        // Defining how tables relate to each other
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ACCOUNT CONFIGURATION
            modelBuilder.Entity<Account>(entity =>
            {
                // Primary Key
                entity.HasKey(e => e.Id);
                // Name is requires, max 100 characters
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                // AccountType stored as an integer (enum value)
                entity.Property(e => e.AccountType)
                    .IsRequired();
                // Balance with precision for money - 18 digits, 2 decimal places
                entity.Property(e => e.Balance)
                    .HasPrecision(18,2)
                    .IsRequired();
                // Currency is requires,exactly 3 characters
                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasMaxLength(3);
                // Timestamps required
                entity.Property(e => e.CreatedAt)
                    .IsRequired();
                entity.Property(e => e.UpdatedAt)
                    .IsRequired();
                // Index on Name for faster searches
                entity.HasIndex(e => e.Name);
                // Index on AccountType for filtering
                entity.HasIndex(e => e.AccountType);

            });

            // CATEGORY CONFIGURATION
            modelBuilder.Entity<Category>(entity =>
            {
                // Primary Key
                entity.HasKey(e => e.Id);
                // Name required, max 50 characters
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
                // Type required (income or expenses)
                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(20);
                // Optional description
                entity.Property(e => e.Description)
                    .HasMaxLength(200);
                // Optional icon/color
                entity.Property(e => e.Icon)
                    .HasMaxLength(50);
                entity.Property(e => e.Color)
                    .HasMaxLength(20);
                // Self-referencing relationship for parent categories
                entity.HasOne<Category>()
                    .WithMany()
                    .HasForeignKey(e => e.ParentCategoryId)
                    .OnDelete(DeleteBehavior.Restrict); // Don't cascade delete
                // Index on Name for searches
                entity.HasIndex(e => e.Name);
                // Index on Type for filtering
                entity.HasIndex(e => e.Type);
            });

            // TRANSACTION CONFIGURATION
            modelBuilder.Entity<Transaction>(entity =>
            {
                // Primary key
                entity.HasKey(e => e.Id);
                // Amount with money precision
                entity.Property(e => e.Amount)
                    .HasPrecision(18, 2)
                    .IsRequired();
                // Currency required
                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasMaxLength(3);
                // Type and Status (enums stored as integers)
                entity.Property(e => e.Type)
                    .IsRequired();
                entity.Property(e => e.Status)
                    .IsRequired();
                // Optional description
                entity.Property(e => e.Description)
                    .HasMaxLength(500);
                // Transaction date required
                entity.Property(e => e.TransactionDate)
                    .IsRequired();
                // Optional fields
                entity.Property(e => e.PaymentMethod)
                    .HasMaxLength(50);

                entity.Property(e => e.ExternalReference)
                    .HasMaxLength(100);

                entity.Property(e => e.Notes)
                    .HasMaxLength(1000);

                entity.Property(e => e.ReceiptUrl)
                    .HasMaxLength(500);

                entity.Property(e => e.Tags)
                    .HasMaxLength(200);

                // Relationships

                // Transaction belongs to an Account
                entity.HasOne(e => e.Account)
                    .WithMany()  // One account has many transactions
                    .HasForeignKey(e => e.AccountId)
                    .OnDelete(DeleteBehavior.Restrict); // Don't delete  transactions if account deleted

                // Transaction belongs to a Category
                entity.HasOne(e => e.Category)
                    .WithMany()
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                // For transfers: optional ToAccount
                entity.HasOne(e => e.ToAccount)
                    .WithMany()
                    .HasForeignKey(e => e.ToAccountId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false); // This is optional

                // Indexes for performance

                // Index on AccountId (most common query: get transactions by account)
                entity.HasIndex(e => e.AccountId);

                // Index on CategoryId (common: get transactions by category)
                entity.HasIndex(e => e.CategoryId);

                // Index on TransactionDate (common: filter by date range)
                entity.HasIndex(e => e.TransactionDate);

                // Index on Status (common: get pending transactions)
                entity.HasIndex(e => e.Status);

                // Index on Type (common: get income vs expenses)
                entity.HasIndex(e => e.Type);

                // Composite index for common queries
                entity.HasIndex(e => new { e.AccountId, e.TransactionDate });

                // Index on IsDeleted for soft delete queries
                entity.HasIndex(e => e.IsDeleted);

                // Query filter: exclude soft-deleted by default
                entity.HasQueryFilter(e => !e.IsDeleted);
            });
        }
    }
}