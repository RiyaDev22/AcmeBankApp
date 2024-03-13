namespace AcmeBank.Accounts;

public abstract class Account
{
    #region Attributes
    private string _name;
    private string _sortCode;
    private string _accountNumber;
    private decimal _balance;
    #endregion

    #region Contructors
    public Account(string name, string sortCode, string accountNumber, decimal balance)
    {
        _name = name;
        _sortCode = sortCode;           //need to ensure length of 6
        _accountNumber = accountNumber; //need to ensure length of 8 (apply formatting 00-00-00)
        _balance = balance;
    }
    #endregion

    #region Getters/Setters
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
    public void Withdraw(int ammount)
    {
        Console.WriteLine($"withdraw {ammount}");
    }

    public void Deposit(int ammount)
    {
        Console.WriteLine($"deposit {ammount}");
    }

    public void Payment()
    {
        /* Payment details
         * ---------------
         * Fullname
         * Sort code
         * Account number
         * Reference */
    }

    public void RequestStatement()
    {
        Console.WriteLine("A statement will be sent to 'name' via 'method'");
    }

    public virtual void DisplayAccountDetails()
    {
        Console.WriteLine($"""
        ----------------------
        Name: {_name}
        Sort Code: {_sortCode} | Account Number: {_accountNumber}
        
        Balance: {_balance:C}
        ----------------------

        """);
    }
    #endregion
}
