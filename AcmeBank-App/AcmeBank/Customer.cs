using AcmeBank.BankAccounts;
using BankPayments.BankAccounts.DerivedAccounts;

namespace AcmeBank;

class Customer
{
    //Setting up attributes of the Customer Class
    private string _firstName;
    private string _otherName;
    private string _lastName;
    private DateOnly _dateOfBirth;
    private DateOnly _customerCreationDate;
    private List<string> _listOfAccounts;
    private bool _hasISA;
    private string _securityQuestion;
    private string _securityAnswer;


    //Constructors

    //Constructor for when ClassCreation() method is run in the Program Class
    public Customer(string firstName, string lastName, string otherName, DateOnly dateOfBirth, string securityQuestion, string securityAnswer)
    {
        this._firstName = firstName;
        this._lastName = lastName;
        this._otherName = otherName;
        this._dateOfBirth = dateOfBirth;
        this._customerCreationDate = DateOnly.FromDateTime(DateTime.Now);
        this._listOfAccounts = new List<string>();
        this._hasISA = false;
        this._securityQuestion = securityQuestion;
        this._securityAnswer = securityAnswer;
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

    public DateOnly CustomerCreationDate
    {
        get { return this._customerCreationDate; }
    }

    public List<string> ListOfAccounts
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

    public string SecurityQuestion
    {
        get { return this._securityQuestion; }
        set { this._securityQuestion = value; }
    }

    public string SecurityAnswer
    {
        get { return this._securityAnswer; }
        set { this._securityAnswer = value; }
    }

    
    //Setting Behaviour for Customer Object
    
    //Adding an account to the Customer Object
    public void AddAccount(Account account) 
    {
        this._listOfAccounts.Add(account.AccountNumber);
        this.checkISA(); //Checks to see if the added account is an ISA account
    }

    //Removing an account in the Customer Object
    public void RemoveAccount(string accountNumber)
    {
        if (this.ValidateAccountRemoval(accountNumber))
        {
            string confirm;
            bool loopCondition = true;
            //Loop to confirm if the user wants to remove the account
            do
            {
                confirm = InputUtilities.StringInputHandling("Are you sure you want to remove your account?").ToLower();
                switch (confirm)
                {
                    case "yes":
                        Console.Clear();
                        this._listOfAccounts.Remove(accountNumber);
                        AccountUtilities.RemoveAccountDetails(accountNumber);
                        Console.WriteLine($"Account {accountNumber} has been removed.");
                        this.checkISA();
                        return;
                    case "no":
                        Console.Clear();
                        Console.WriteLine($"Account {accountNumber} has not been removed.");
                        return;
                    default:
                        Console.Clear();
                        Console.WriteLine("Make sure to answer with 'Yes' or 'No'");
                        break;
                }
            } while (loopCondition);
            
        }
        else
        {
            Console.WriteLine($"Account {accountNumber} has not been able to be removed.");
        }
    }

    //TODO!!! Complete this method once standing order/direct debits have been implemented
    //Helper function for RemoveAccount
    private bool ValidateAccountRemoval(string accountNumber)
    {
        //Need inner working of account class to finish this function
        if (this._listOfAccounts.Contains(accountNumber))
        {
            Account account = AccountUtilities.LoadAccountDetails(accountNumber);
            
            AccountType accountType= account.Type;
            
            switch (accountType)
            {
                case AccountType.Personal:
                    PersonalAccount personal = (PersonalAccount) account;
                    return personal.OverdraftRemaining < 0;
                case AccountType.ISA:
                    return true;
                case AccountType.Business:
                    BusinessAccount business = (BusinessAccount) account;
                    return business.OverdraftRemaining < 0;
                default:
                    return false;
            }
        }
        return false;
    }

    //Checks if the Customer object contains any accounts of type ISA. WIll change _hasISA based on the outcome
    private void checkISA()
    {
        foreach (string i in this._listOfAccounts)
        {
            Account account = AccountUtilities.LoadAccountDetails(i);
            if (account.GetType() == typeof(ISAAccount))
            {
                this._hasISA = true;
                return;
            }
            this._hasISA = false;
        }
    } 
}
