namespace AcmeBank;

class Customer
{
    //Setting up attributes of the Customer Class
    private string _firstName;
    private string _otherName;
    private string _lastName;
    private DateOnly _dateOfBirth;
    private DateOnly _accountCreationDate;
    private List<Account> _listOfAccounts;
    private bool _hasISA;


    //Constructors

    //Constructor for when AccountCreation() method is run in the Program Class
    public Customer(string firstName, string lastName, DateOnly dateOfBirth, string otherName = "")
    {
        _firstName = firstName;
        _lastName = lastName;
        _otherName = otherName;
        _dateOfBirth = dateOfBirth;
        _accountCreationDate = DateOnly.FromDateTime(DateTime.Now);
        _listOfAccounts = new List<Account>();
        _hasISA = false;
    }

    //Getters and Setters for Attributes of the Customer Object

    public string FirstName
    {
        get { return this._firstName; }
        set { this._firstName = value; }
    }

    public string LastName
    {
        get { return this._lastName; }
        set { this._lastName = value; }
    }

    public string OtherName
    {
        get { return this._otherName; }
        set { this._lastName = value; }
    }

    public DateOnly DateOfBirth
    {
        get { return this._dateOfBirth; }
    }

    public DateOnly AccountCreationDate
    {
        get { return this._accountCreationDate; }
    }

    public List<Account> ListOfAccounts
    {
        get { return this._listOfAccounts; }
    }

    public bool HasISA 
    { 
        get
        {
            this.checkISA();
            return this._hasISA;
        }
    }


    //Setting Behaviour for Customer Object
    
    //Adding an account to the Customer Object
    public void AddAccount(Account account) 
    {
        this._listOfAccounts.Add(account);
        this.checkISA(); //Checks to see if the added account is an ISA account
    }

    //removing an account in the Customer Object
    public void RemoveAccount(Account account)
    {
        if (this.ValidateAccountRemoval(account))
        {
            Console.WriteLine("Account has been removed.");
            this._listOfAccounts.Remove(account);
            this.checkISA();
        }
        else
        {
            Console.WriteLine("Account has not been able to be removed.");
        }
    }

    //Helper function for RemoveAccount
    private bool ValidateAccountRemoval(Account account)
    {
        //Need inner working of account class to finish this function
        if (this._listOfAccounts.Contains(account))
        {
            return true;
        }
        return false;
    }

    //Checks if the Customer object contains any accounts of type ISA. WIll change _hasISA based on the outcome
    private void checkISA()
    {
        //WIll be able to implement once the ISA Class has been implemented
        throw new NotSupportedException();
    } 
}
