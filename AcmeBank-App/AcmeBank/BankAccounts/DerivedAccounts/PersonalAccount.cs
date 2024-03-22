using AcmeBank;
using AcmeBank.BankAccounts;
using AcmeBank.BankAccounts.AccountInterfaces;
using AcmeBank.BankAccounts.RegularPayments;
using AcmeBank.BankAccounts.Transactions;
using System.Text;

namespace BankPayments.BankAccounts.DerivedAccounts;

public class PersonalAccount : Account, IOverdraftAccount
{
    #region Attributes
    private decimal _overdraftRemaining;
    private const decimal _overdraftLimit = 2_000m; // Used as a constant for now. This should be adjustable
    #endregion

    #region Constructors
    // Account setup
    public PersonalAccount(string accountNumber, string sortCode, decimal balance, string address, Customer customer) : base(accountNumber, sortCode, balance, AccountType.Personal, address, customer)
    {
        _overdraftRemaining = _overdraftLimit;
    }
    // Loading from file
    public PersonalAccount(string accountNumber, string sortCode, decimal balance, string address, decimal overdraftRemaining, Customer customer) : base(accountNumber, sortCode, balance, AccountType.Personal, address, customer)
    {
        _overdraftRemaining = overdraftRemaining; 
    }
    #endregion

    #region Getters/Setters
    // Gets the overdraft limit.
    public decimal OverdraftLimit { get { return _overdraftLimit; } }

    // Gets or sets the remaining overdraft amount.
    public decimal OverdraftRemaining
    {
        get { { return _overdraftRemaining; } }
        set
        {
            // Ensure the new value is within the range 0 and the limit. This is primarily used for depositing as will prevent the limit from increasing past the limit
            _overdraftRemaining = Math.Clamp(value, 0, OverdraftLimit);
        }
    }
    #endregion

    #region Methods

    // Display account options
    protected override void DisplayAccountOptions()
    {
        Console.WriteLine("""

            --- Account options ---
            1. Deposit
            2. Withdraw
            3. Payment
            4. Transfer
            5. Statement
            6. Manage Standing Orders/Direct Debits
            7. Request Debit Card
            8. Manage Overdraft

            x. Exit
            -----------------------
            """);
    }

    // Handle account options
    protected override bool HandleOption(string option, ref StringBuilder invalidPrompt)
    {
        // Handle the selected option
        switch (option)
        {
            case "1":
                Deposit();
                break;
            case "2":
                Withdraw();
                break;
            case "3":
                Payment();
                break;
            case "4":
                Transfer();
                break;
            case "5":
                Statements.StatementOptions(AccountNumber, CustomerReference);
                break;
            case "6":
                // Standing Orders & Direct Debits
                RegularPaymentUtilities.RegularPaymentOptions(this, CustomerReference);
                break;
            case "7":
                RequestDebitCard();
                break;
            case "8":
                ManageOverdraft();
                break;
            case "x":
                // Exit if the user selects 'x'
                return true;
            default:
                // if the user selects an invalid option, set the text for the invalid prompt to be shown next time
                invalidPrompt.Append("-- !!! Invalid option !!! --");
                return false;
        }
        return false;
    }

    // Deduct amount from the overdraft remaining
    public bool UpdateRemainingOverdraft(decimal amount)
    {
        if (Balance > 0) // If the balance is positive, deduct it from the amount to be withdrawn from overdraft
            amount -= Balance;

        if (OverdraftRemaining - amount < 0) // Check if the new overdraft amount will be negative after the update
            return false;
        else
            OverdraftRemaining -= amount;

        return true;
    }

    // Displays account information and additonally the overdraft limit
    public override void DisplayAccountDetails()
    {
        // Display account details as well as overdraft
        Console.WriteLine($"""
            --- Account details ---
            Account Number: {AccountNumber}
            Sort Code: {SortCode}
            Balance: {Balance:C2}
            Type: {Type} Account
            -
            Overdraft limit: {OverdraftLimit:C2}
            Overdraft remaining: {OverdraftRemaining:C2}
            -----------------------
            """);
    }
    
    // Request a new debit card
    // This method will simply inform the user that a new debit card will be sent to the account's address
    protected void RequestDebitCard()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"A new debit card will be sent to this account's address at {Address}");
        Console.ResetColor();
        Thread.Sleep(1500);
    }
    
    // Manage the account's overdraft
    protected void ManageOverdraft()
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Please direct the customer to our Overdraft division");
        Console.ResetColor();
        Thread.Sleep(1500);
    }
    
    #endregion

}
