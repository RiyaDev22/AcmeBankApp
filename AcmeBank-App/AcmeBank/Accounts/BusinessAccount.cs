namespace AcmeBank.Accounts;

public class BusinessAccount : Account
{
    #region Attributes
    // Attribute for overdraft limit
    private decimal _overdraftLimit;
    #endregion

    #region Constructors
    // Constructor to initialize business account details
    public BusinessAccount(string name, string sortCode, string accountNumber, decimal balance, decimal overdraftLimit) : base(name, sortCode, accountNumber, balance)
    {
        _overdraftLimit = overdraftLimit;
    }
    #endregion

    #region Getters/Setters
    // Property to access and modify overdraft limit
    public decimal OverdraftLimit
    {
        get { return _overdraftLimit; }
        set { _overdraftLimit = value; } // Can realistically change but not sure for this context
    }
    #endregion

    #region Methods
    // Method to request a cheque book
    public void RequestChequeBook()
    {
        // Display information about the cheque book request
        Console.WriteLine("""
            A cheque book has been sent to:
              name:
              address:

            Press enter to continue.
            """);
        // Allows user to read the prompt
        Console.ReadLine();
    }

    // Method to perform a loan eligibility check
    public void LoanEligibilityCheck()
    {
        /* Actual example Lloyds
         * Customers have risk indicator 1 - 9, (1 being the best - 9 being the worst)
         * This displays how much they would be able to borrow #(over scope)#
         */

        // Display information about loan department contact details
        Console.WriteLine("""
            Please refer the customer to:
              ---- Loan Department ---
              email: 
              phone:

            Press enter to continue.
            """);
        // Allows user to read the prompt
        Console.ReadLine();
    }

    // Override method to display account details with business account-specific information
    public override void DisplayAccountDetails()
    {
        // Display account details with business account-specific formatting
        Console.WriteLine($"""
            ----------------------
            Name: {Name}
            Sort Code: {SortCode} | Account Number: {AccountNumber}
            #
            Balance: {Balance:C}
            #
            Overdraft limit: {_overdraftLimit:C}
            ----------------------

            """);
    }
    #endregion
}