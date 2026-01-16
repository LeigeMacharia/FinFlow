using System;

namespace FinFlow.Core.Models
{
    // Represents a transaction category (eg. Groceries, salary, rent, entertainment)
    // Used to organize and analyze spending/income patterns
    public class Category
    {
        // Unique Identifier
        public Guid Id { get; set; }
        // Category name (Food & Dining, Business name)
        public string Name { get; set; }
        //Type: Income or Expense
        // Determines if this category increases or decreases balance
        public string Type { get; set; }
        // Optional: Parent category for hierarchical structure
        // Example: Restaurant --- Food & Dinning
        public Guid? ParentCategoryId { get; set; }
        // Description of what this category is for
        public string? Description { get; set; }
        // Icon or color for UI display
        public string? Icon { get; set; }
        public string? Color { get; set; }
        //Timestamps
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        // Is this category active oe archived?
        public bool IsActive { get; set; }

        // Constructor
        public Category()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            IsActive = true;
            Name = string.Empty;
            Type= "expense";
        } 

        // Validates category has required data
        public bool IsValid()
        {
            // Name is required
            if (string.IsNullOrWhiteSpace(Name))
                return false;
            // Name length
            if (Name.Length < 2 || Name.Length > 50)
                return false;
            //Type must ne "income" or "expense"
            if (Type != "income" && Type != "expense")
                return false;
            
            return true;
        }
    }
}