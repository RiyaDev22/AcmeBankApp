using AcmeBank;
using AcmeBank.BankAccounts;
using AcmeBank.BankAccounts.AccountInterfaces;
using System.Text;

namespace BankPayments.BankAccounts.DerivedAccounts;

public class BusinessAccount : Account, IOverdraftAccount
{
    #region Attributes
    private decimal _overdraftRemaining;
    private const decimal _overdraftLimit = 2_000m; // Used as a constant for now. This should be adjustable
    #endregion

    #region Constructors
    // Account setup
    public BusinessAccount(string accountNumber, string sortCode, decimal balance, string address) : base(accountNumber, sortCode, balance, AccountType.Business, address)
    {
        _overdraftRemaining = _overdraftLimit; 
    }
    // Loading from file
    public BusinessAccount(string accountNumber, string sortCode, decimal balance, string address, decimal overdraftRemaining) : base(accountNumber, sortCode, balance, AccountType.Business, address)
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

    protected override void DisplayAccountOptions()
    {
        Console.WriteLine("""
            --- Account options ---
            1. Deposit
            2. Withdraw
            3. Payment
            4. Transfer
            5. Request Credit/Debit Card
            6. Request Cheque Book
            7. Manage Loans
            8. Manage Overdraft

            X. Exit
            -----------------------
            """);
    }

    protected override bool HandleOption(string option, ref StringBuilder invalidPrompt)
    {
        // Process the user's input
        switch (option.ToLower())
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
                RequestCard();
                break;
            case "6":
                RequestChequeBook();
                break;
            case "7":
                ManageLoans();
                break;
            case "8":
                ManageOverdraft();
                break;
            // exit the loop if the user chooses to exit
            case "x":
                return true;
            default:
                // display an error message if the user enters an invalid option
                Console.Clear();
                invalidPrompt.Append("-- !!! Invalid option !!! --");
                break;
        }
        return false;
    }

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

    protected void RequestCard()
    {
        //we need a submenu for handling the request of a credit or debit card, since the customer could request either
        string? cardInput;
        bool exit = false;
        //we will use a stringbuilder to store the invalid prompt message
        StringBuilder invalidPrompt = new StringBuilder();
        //loop until the user chooses to exit
        while (!exit)
        {
            //show the menu and output the invalid prompt if there is one
            Console.Clear();
            Console.WriteLine("------Card Request-----");
            Console.WriteLine("1. Request Credit Card");
            Console.WriteLine("2. Request Debit Card");
            Console.WriteLine("X. Exit");
            Console.WriteLine("-----------------------");

            //if there is an invalid prompt, display it in red.
            //if there is no prompt, this will simply show as an empty line
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(invalidPrompt.ToString());

            //we reset the console afterward
            Console.ResetColor();
            invalidPrompt.Clear();

            //get the user's input
            Console.Write("Enter an option: ");
            cardInput = InputUtilities.GetInputWithinTimeLimit();

            //process the user's input
            switch(cardInput.ToLower())
            {
                //if the user chooses to request a credit card, tell them one will be shipped
                case "1":
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"A new credit card will be sent to this account's address at {Address}");
                    Console.ResetColor();
                    Thread.Sleep(1500);
                    exit = true;
                    break;
                //same as above, but for a debit card
                case "2":
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"A new debit card will be sent to this account's address at {Address}");
                    Console.ResetColor();
                    Thread.Sleep(1500);
                    exit = true;
                    break;
                //if the user chooses to exit, set the exit flag to true
                case "x":
                    exit = true;
                    break;
                //if the user enters an invalid option, add it to the invalid prompt to be shown on the next loop
                default:
                    Console.Clear();
                    invalidPrompt.Append("-- !!! Invalid option !!! --");
                    Console.WriteLine(invalidPrompt.ToString());
                    break;
            }
        }
    }

    // Request a cheque book
    // this will simply inform the user that a new cheque book will be sent to their address
    protected void RequestChequeBook()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"A new cheque book will be sent to this account's address at {Address}");
        Console.ResetColor();
        Thread.Sleep(2500);
    }

    // Manage loans
    // this is simply an instruction to the teller to direct the customer's inquiry to the appropriate department
    protected void ManageLoans()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Please direct the customer to our Business Loans division");
        Console.ResetColor();
        Thread.Sleep(3000);
    }

    // The same goes for managing overdrafts
    protected void ManageOverdraft()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Please direct the customer to our Business Overdraft division");
        Console.ResetColor();
        Thread.Sleep(3000);
    }
    #endregion
}
