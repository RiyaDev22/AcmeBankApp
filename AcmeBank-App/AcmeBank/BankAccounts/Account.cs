using AcmeBank.BankAccounts.AccountInterfaces;
using AcmeBank.BankAccounts.Transactions;
using System.Text;

namespace AcmeBank.BankAccounts;

public abstract class Account
{
    #region Attributes
    private string _accountNumber;
    private string _sortCode;
    private decimal _balance;
    private AccountType _type;
    private string _address;
    private Customer _customerReference;
    #endregion

    #region Constructors
    public Account(string accountNumber, string sortCode, decimal balance, AccountType accountType, string address, Customer customer)
    {
        _accountNumber = accountNumber;
        _sortCode = sortCode;
        _balance = balance;
        _type = accountType;
        _address = address;
        _customerReference = customer;
    }
    #endregion

    #region Getters/Setters
    public string AccountNumber { get { return _accountNumber; } }
    public string SortCode { get { return _sortCode; } }
    public decimal Balance 
    { 
        get { return _balance; } 
        set { _balance = value; }
    }
    public AccountType Type { get { return _type; } }

    public string Address { get { return _address; } }

    public Customer CustomerReference
    { 
        get { return _customerReference; }
        set { _customerReference = value; }
    }
    #endregion

    #region Methods
    // Displays a menu of options
    protected virtual void DisplayAccountOptions()
    {
        Console.WriteLine("""
                --- Account options ---
                1. Deposit
                2. Withdraw
                3. Payment
                4. Transfer
                5. Statement
                X. Exit
                -----------------------
                """);
    }

    public virtual void AccountOptionsLoop()
    {
        StringBuilder invalidPrompt = new StringBuilder();

        bool exit = false; // Initialize a flag to control the loop
        while (!exit) // Loop until the user chooses to exit
        {
            // Display account details and options
            Console.Clear();
            DisplayAccountDetails();
            DisplayAccountOptions();

            // Display any error messages
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(invalidPrompt.ToString());
            Console.ResetColor();
            invalidPrompt.Clear();

            // Ask the user to enter an option
            Console.Write("Enter an option: ");
            string optionInput = Console.ReadLine();

            exit = HandleOption(optionInput, ref invalidPrompt);
        }
    }

    protected virtual bool HandleOption(string option, ref StringBuilder invalidPrompt)
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
                Statements.StatementOptions(AccountNumber, CustomerReference);
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

    public virtual void DisplayAccountDetails()
    {
        Console.WriteLine($"""
            --- Account details ---
            Account Number: {AccountNumber}
            Sort Code: {SortCode}
            Balance: {Balance:C}
            Type: {Type} Account
            -----------------------
            """);
    }

    protected virtual void Deposit()
    {
        // Initialize variables to store user input and error messages
        string? input;
        decimal amount = 0;
        StringBuilder invalidPrompt = new StringBuilder();

        // Loop until a valid deposit amount is entered
        do
        {
            // Display account details and deposit header
            Console.Clear();
            DisplayAccountDetails();
            Console.WriteLine("""
                ------- Deposit -------
                """);

            // Display any previous error messages
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(invalidPrompt.ToString());
            Console.ResetColor();
            invalidPrompt.Clear();

            // Ask for input
            Console.Write("Enter an amount: ");
            input = Console.ReadLine();

        }while(!ValidateDepositInput(ref amount, input, ref invalidPrompt)); // Repeat loop until the deposit amount is valid

        AddToBalance(amount, TransactionType.Deposit); // Add the validated deposit amount to the account balance
    }

    protected void Withdraw()
    {
        // Check if there are sufficient funds in the account before proceeding with the withdrawal
        if (!CheckSufficientFunds()) { return; }

        // Initialize variables to store user input and error messages
        string? input;
        decimal amount = 0;
        StringBuilder invalidPrompt = new StringBuilder();

        // Loop until a valid withdrawal amount is entered
        do
        {
            // Display account details and withdraw header
            Console.Clear();
            DisplayAccountDetails();
            Console.WriteLine("""
                ------- Withdraw ------
                """);

            // Display any previous error messages
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(invalidPrompt.ToString());
            Console.ResetColor();
            invalidPrompt.Clear();

            // Ask for input
            Console.Write("Enter an amount: ");
            input = Console.ReadLine();

        } while (!ValidateWithdrawInput(ref amount, input, ref invalidPrompt)); // Repeat loop until the withdrawal amount is valid

        // If the account implements deposit limit functionality, update the deposit limit
        if (this is IDepositLimitedAccount depositLimitedAccount)
            depositLimitedAccount.UpdateDepositLimit(-amount);

        DeductFromBalance(amount, TransactionType.Withdraw); // Deduct the validated withdrawal amount from the account balance
    }

    protected void Payment()
    {
        // Check if there are sufficient funds in the account before proceeding with the payment
        if (!CheckSufficientFunds()) { return; }

        // Initialize variables for payee account details, payment amount, and error messages
        Account payeeAccount = null;
        string? input;
        decimal amount = 0;
        StringBuilder invalidPrompt = new StringBuilder();
        List<string> invalidAccountNumbers = new List<string>() { AccountNumber }; // This is a list of a accounts we cannot pay into e.g the customers own accounts.

        // Loop until a valid payee account is selected and a valid payment amount is entered
        do
        {
            // Get payee details (sort code and account number)
            TransactionUtilities.GetPayeeDetails(out string sortCode, out string accountNumber,invalidAccountNumbers);
            payeeAccount = AccountUtilities.LoadAccountDetails($"{accountNumber}", CustomerReference); // Load payee account details based on the provided account number

            // Checks if the payee is a savings account if so we provide an error prompt and return preventing the payment
            if (payeeAccount.Type == AccountType.ISA)
            {
                payeeAccount = null;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("!!! Can not make a payment to a savings account !!!");
                Console.ResetColor();

                Thread.Sleep(1500); // Pause execution briefly to display the error message
                Console.Clear();
            }

        } while (payeeAccount == null);

        do
        {
            // Display account details and payment header including account from and to
            Console.Clear();
            DisplayAccountDetails();
            Console.WriteLine($"""
                ------- Payment -------
                From: {this.AccountNumber}
                To: {payeeAccount.AccountNumber}
                -----------------------
                """);

            // Display any previous error messages
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(invalidPrompt.ToString());
            Console.ResetColor();
            invalidPrompt.Clear();

            // Ask for input
            Console.Write("Enter an amount: ");
            input = Console.ReadLine();

        } while (!ValidateWithdrawInput(ref amount, input, ref invalidPrompt) || !payeeAccount.ValidateDepositInput(ref amount, input, ref invalidPrompt)); // Repeat loop until both withdrawal and deposit validations pass

        //ask for reference
        //Regex.IsMatch(userInput, @"^(?![,\d\s]*$)[^\d,]*$")
        /*^ and $ ensure that the entire string matches the pattern.
         * (?![,\d\s]*$) is a negative lookahead assertion that ensures the string doesn't consist only of commas, digits, and spaces. This prevents empty strings as well.
         *[^\d,]* matches any character that is not a digit or comma, ensuring that commas and numbers are not allowed. */

        //Console.WriteLine($"""
        //    ------- Payment -------
        //    From: {this.AccountNumber}
        //    To: {payeeAccount.AccountNumber}
        //    Amount: {amount:C}
        //    -----------------------
        //    """);

        //could confirm payment here

        // Deduct the payment amount from the sender's account
        this.DeductFromBalance(amount,TransactionType.Payment);

        Console.WriteLine();
        // Add the payment amount to the payee's account
        payeeAccount.AddToBalance(amount, TransactionType.Payment);

        // Display payment success message
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Payment successful!");
        Console.ResetColor();
        Thread.Sleep(1000);
    }

    protected void Transfer()
    {
        //get customer accounts
        List<string> accountNumbers = CustomerReference.ListOfAccounts;
        //list accounts
        accountNumbers.Remove(AccountNumber);

        List<Account> accountObjects = new List<Account>();
        StringBuilder invalidOptionPrompt = new StringBuilder();

        bool exit = false;
        while (!exit)
        {
            //ask for option
            Console.Clear();
            Console.Write("""
            --- Transfer: Account Selection ---
            Please enter the ID or account number to select
            Enter 'x' to exit.
            
            Enter: 
            """);

            // Save the current cursor position
            int currentLeft = Console.CursorLeft;
            int currentTop = Console.CursorTop;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n{invalidOptionPrompt}");
            invalidOptionPrompt.Clear();
            Console.ResetColor();

            Console.WriteLine("=======================");
            int count = 1;
            foreach (string accountNumber in accountNumbers)
            {
                Account accountObject = AccountUtilities.LoadAccountDetails(accountNumber, CustomerReference);
                accountObjects.Add(accountObject);

                Console.WriteLine($"""
                
                -------- ID: {count,2} ------- 
                """);
                accountObject.DisplayAccountDetails();
                count++;
            }
            Console.WriteLine("\n=======================");

            // Move the cursor position to the line where user input is expected
            Console.SetCursorPosition(currentLeft, currentTop);

            // Then provide the option to send statement or exit
            string? input = Console.ReadLine();
            int id;
            bool validID = int.TryParse(input, out id);
            if (input.ToLower() == "x")
            {
                exit = true;
            } 
            else if (validID && id > 0 && id <= accountNumbers.Count)
            {
                TransferAmount(accountNumbers[id-1]);
            }
            else if(accountNumbers.Contains(input))
            {
                TransferAmount(input);
            }
            else
            {
                invalidOptionPrompt.AppendLine("!!! Invalid ID !!!");
            }
        }
    }

    private void TransferAmount(string accountNumber)
    {
        Account TransferToAccount = AccountUtilities.LoadAccountDetails($"{accountNumber}", CustomerReference);
        decimal amount =0;

        StringBuilder invalidPrompt = new StringBuilder();
        string? input;
        do
        {
            // Display account details and payment header including account from and to
            Console.Clear();
            DisplayAccountDetails();
            Console.WriteLine($"""

                ------- Transfer ------
                From: {this.AccountNumber}
                To: {TransferToAccount.AccountNumber}
                -----------------------
                """);

            // Display any previous error messages
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(invalidPrompt.ToString());
            Console.ResetColor();
            invalidPrompt.Clear();

            // Ask for input
            Console.Write("Enter an amount: ");
            input = Console.ReadLine();

        } while (!ValidateWithdrawInput(ref amount, input, ref invalidPrompt) || !TransferToAccount.ValidateDepositInput(ref amount, input, ref invalidPrompt));

        // Deduct the payment amount from the sender's account
        this.DeductFromBalance(amount, TransactionType.Transfer);

        Console.WriteLine();
        // Add the payment amount to the payee's account
        TransferToAccount.AddToBalance(amount, TransactionType.Transfer);

        // Display payment success message
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Transfer successful!");
        Console.ResetColor();
        Thread.Sleep(1000);
    }

    protected void DeductFromBalance(decimal amount, TransactionType transactionType)
    {
        // Deduct funds from the account balance
        _balance -= amount;

        // Save the updated account details
        AccountUtilities.SaveAccountDetails(this);

        // Create a transaction log for the withdrawal and save it to the transaction file
        Transaction withdraw = new Transaction(-amount, _balance, transactionType, DateTime.Now);
        AccountUtilities.SaveTransaction(withdraw, AccountNumber);
    }

    protected void AddToBalance(decimal amount, TransactionType transactionType)
    {
        // If the account implements overdraft functionality, update the remaining overdraft
        if (this is IOverdraftAccount overdraft)
            overdraft.UpdateRemainingOverdraft(-amount);

        // Add funds to the account balance
        _balance += amount;

        // Save the updated account details
        AccountUtilities.SaveAccountDetails(this);

        // Create a transaction log for the deposit and save it to the transaction file
        Transaction deposit = new Transaction(amount, _balance, transactionType, DateTime.Now);
        AccountUtilities.SaveTransaction(deposit, AccountNumber);
    }

    private bool CheckSufficientFunds()
    {
        // Check if the account has sufficient funds for the requested transaction
        if ((this is IOverdraftAccount overdraftAccount && overdraftAccount.OverdraftRemaining <= 0) || (this is not IOverdraftAccount && Balance <= 0))
        {
            // Display an error message for insufficient funds
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("!!! Insufficient funds  !!!");
            Console.ResetColor();

            Thread.Sleep(1500); // Pause execution briefly to display the error message
            Console.Clear();
            return false;
        }
        return true;
    }

    protected bool ValidateWithdrawInput(ref decimal amount, string? input, ref StringBuilder invalidPrompt)
    {
        if (!decimal.TryParse(input, out amount)) // Validate input is a decimal number
        {
            invalidPrompt.Append("!!! invalid input !!!");
        } 
        else if (amount <= 0) // Validate that its greater than zero
        {
            invalidPrompt.Append("!!! Must be greater than zero !!!");
        } 
        else if (this is IOverdraftAccount overdraft && !overdraft.UpdateRemainingOverdraft(amount)) // Validate withdrawal does not exceed overdraft limit
        {
            invalidPrompt.Append($"!!! Must not exceed overdraft !!!");
        } 
        else if (this is not IOverdraftAccount && Balance - amount < 0) // Validate sufficient funds. 
        {
            invalidPrompt.Append("!!! Insufficient funds !!!");
        } 
        else
        {
            return true;
        }
        return false;
    }

    protected bool ValidateDepositInput(ref decimal amount, string? input, ref StringBuilder invalidPrompt)
    {
        if (!decimal.TryParse(input, out amount)) // Validate input is a decimal number
        {
            invalidPrompt.Append("!!! invalid input !!!");
        } 
        else if (amount <= 0) // Validate that its greater than zero
        {
            invalidPrompt.Append("!!! Must be greater than zero !!!");
        } 
        else if (this is IDepositLimitedAccount depositLimitedAccount && !depositLimitedAccount.UpdateDepositLimit(amount)) // Validate deposit does not exceed deposit limit for ISA (£20,000)
        {
            invalidPrompt.Append($"!!! Exceeded deposit limit. Maximum deposit allowed: {depositLimitedAccount.RemainingDepositLimit:C} !!!");
        } 
        else
        {
            return true;
        }
        return false;
    }


    #endregion
}
