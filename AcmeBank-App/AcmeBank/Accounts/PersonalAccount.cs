using AcmeBank.Utility;

namespace AcmeBank.Accounts;

public class PersonalAccount : Account
{
    #region Attributes

    #endregion

    #region Contructors
    // Constructor to initialize personal account details with a minimum opening balance of £1
    public PersonalAccount(string name, string sortCode, string accountNumber) : base(name, sortCode, accountNumber, 0m)
    {
        // Prompt message for the initial deposit
        string validDepositPrompt = """
        This account requires a minimum opening balance of £1
        Please enter the amount to Deposit

        """;

        decimal amount; // Variable to hold the validated deposit amount
        bool initalPass = true; // Flag to determine if it's the initial pass through the loop

        // Loop until a valid deposit amount greater than or equal to £1 is provided
        do
        {
            // Prompt the user for an initial deposit
            // If it's not the initial pass and so less than 1, display an error message
            amount = InputValidator.TryGetDecimalInput(validDepositPrompt, initalPass? "":"-! Must be a minimum of £1 !-\n");

            initalPass = false; // Update the initialPass flag after the first pass
        } while (amount < 1m); // Repeat until a valid amount is provided

        // Deposit the validated amount into the account
        this.Deposit(amount);
    }

    #endregion

    #region Getters/Setters
    
    #endregion

    #region Methods
    // Override method to display account details with personal account-specific information
    public override void DisplayAccountDetails()
    {
        // Display account details with personal account-specific formatting
        Console.WriteLine($"""
        ----------------------
        {Name}
        Sort Code: {SortCode} | Account Number: {AccountNumber}
        #
        Balance: {Balance:C}
        ----------------------

        """);
    }
    #endregion
}
