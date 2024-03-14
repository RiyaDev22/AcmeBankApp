namespace AcmeBank.Accounts;

public class ISAAccount : Account
{
    #region Attributes
    // Constants for interest rate and tax-free limit
    private const decimal _interestRate = 0.0275m;
    private const decimal _taxFreeLimit = 20_000m;
    #endregion

    #region Constructors
    // Constructor to initialize ISA account details
    public ISAAccount(string name, string sortCode, string accountNumber, decimal balance) : base(name, sortCode, accountNumber, balance)
    {

    }
    #endregion

    #region Getters/Setters
    
    #endregion

    #region Methods
    // Method to calculate Annual Percentage Rate (APR) for the account
    private decimal CalculateAPR()
    {
        return 0m; // Placeholder implementation
    }

    // Override method to display account details with ISA-specific information
    public override void DisplayAccountDetails()
    {
        // Display account details with ISA-specific formatting
        //base.DisplayAccountDetails();
        Console.WriteLine($"""
            ----------------------
            {Name}
            Sort Code: {SortCode} | Account Number: {AccountNumber}
            #
            Balance: {Balance:C}
            #
            Interest rate: {_interestRate:P2}
            ----------------------

            """);
    }
    #endregion
}
