using AcmeBank.Utility;

namespace AcmeBank.Accounts;

// Abstract class representing a bank account
public abstract class Account
{
    #region Attributes
    // Private attributes for account details
    private string _name;
    private string _sortCode;
    private string _accountNumber;
    private decimal _balance;
    #endregion

    #region Constructors
    // Constructor to initialize account details
    public Account(string name, string sortCode, string accountNumber, decimal balance)
    {
        _name = name;
        _sortCode = sortCode;           // Need to ensure length of 6
        _accountNumber = accountNumber; // Need to ensure length of 8 (apply formatting 00-00-00)
        _balance = balance;
    }
    #endregion

    #region Getters/Setters
    // Properties to access account details
    public string Name { get { return _name; } }
    public string SortCode { get { return _sortCode; } }
    public string AccountNumber { get { return _accountNumber; } }
    public decimal Balance
    {
        get { return _balance; }
        set { _balance = value; }
    }
    #endregion

    #region Methods
    // Method to withdraw funds from the account
    public virtual void Withdraw(decimal amount)
    {
        // Need to ensure there's a sufficient amount or overdraft
        Balance -= amount;
    }

    // Method to deposit funds into the account
    public virtual void Deposit(decimal amount)
    {
        // You can add as much as you like to an ISA but the first 20k is tax free
        Balance += amount;
    }

    // Placeholder for payment details
    public void Payment()
    {
        /* Payment details
         * ---------------
         * Fullname
         * Sort code
         * Account number
         * Reference */
    }

    // Method to request a statement
    public void RequestStatement()
    {
        Console.WriteLine("A statement will be sent to 'name' via 'method'");
    }

    // Abstract method to display account details (implementation required by subclasses)
    public abstract void DisplayAccountDetails();
    #endregion
}