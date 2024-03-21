using AcmeBank.BankAccounts;
using AcmeBank.BankAccounts.AccountInterfaces;
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
    public PersonalAccount(string accountNumber, string sortCode, decimal balance, string address) : base(accountNumber, sortCode, balance, AccountType.Personal, address)
    {
        _overdraftRemaining = _overdraftLimit;
    }
    // Loading from file
    public PersonalAccount(string accountNumber, string sortCode, decimal balance, string address, decimal overdraftRemaining) : base(accountNumber, sortCode, balance, AccountType.Personal, address)
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

    protected override void DisplayAccountOptions()
    {
        Console.WriteLine("""
                --- Account options ---
                1. Deposit
                2. Withdraw
                3. Payment
                4. Transfer
                5. Statement
                6. Regular Payments
                X. Exit
                -----------------------
                """);
    }
    protected override bool HandleOption(string option, ref StringBuilder invalidPrompt)
    {
        switch (option.ToLower()) // Process the user's choice
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
                Statements.StatementOptions(AccountNumber);
                break;
            case "6":
                // Standing Orders & Direct Debits
                RegularPayments.RegularPaymentOptions(this);
                break;
            case "x":
                // Exit the loop if the user chooses to exit
                return true;
            default:
                // Display an error message if the user enters an invalid option
                Console.Clear();
                invalidPrompt.Append("-- !!! Invalid option !!! --");
                break;
        }
        return false; //does not exit the loop
    }


    #endregion

}
