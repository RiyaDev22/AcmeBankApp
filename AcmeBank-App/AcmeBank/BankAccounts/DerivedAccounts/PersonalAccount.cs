using AcmeBank.BankAccounts;
using AcmeBank.BankAccounts.AccountInterfaces;

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
            Balance: {Balance:C}
            Type: {Type} Account
            -
            Overdraft limit: {OverdraftLimit:C}
            Overdraft remaining: {OverdraftRemaining:C}
            -----------------------

            """);
    }
    #endregion

}
