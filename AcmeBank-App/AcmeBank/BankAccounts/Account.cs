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

    public Customer CustomerReference { get { return _customerReference; } }
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

    // Takes an input from the user
    public virtual void AccountOptionsLoop()
    {
        StringBuilder invalidPrompt = new StringBuilder(); // StringBuilder to store error messages

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
            string optionInput = InputUtilities.GetInputWithinTimeLimit();

            exit = HandleOption(optionInput, ref invalidPrompt); // Triggers a method based on the selected option
        }
    }

    // Performs an action depending on the input given
    protected virtual bool HandleOption(string option, ref StringBuilder invalidPrompt)
    {
        switch (option.ToLower()) // Process the user's choice
        {
            case "1":
                Deposit(); // Deposit funds to this account
                break;
            case "2":
                Withdraw(); // Withdraw funds from this account
                break;
            case "3":
                Payment(); // Make a payment from this account to another
                break;
            case "4":
                Transfer(); // Transfer funds accross owned accounts
                break;
            case "5":
                // Call method to display account statements given a specific month and year
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
        return false; // Does not exit the loop
    }

    // Display account details including account number, sort code, balance, and account type
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

    // Deposit method allows the user to deposit funds into the account.
    // The user is prompted to enter the deposit amount, and the amount is validated.
    // If the amount is valid, it is added to the account balance.
    protected virtual void Deposit()
    {
        // Initialize variables to store user input and error messages
        string? input;
        decimal amount = 0;
        StringBuilder invalidPrompt = new StringBuilder();
        StringBuilder helpPrompt = new StringBuilder();

        bool exit = false;
        // Loop until a valid deposit amount is entered
        do
        {
            // Display account details and deposit header
            Console.Clear();
            DisplayAccountDetails();
            Console.WriteLine("""

                ------- Deposit -------
                ## Provide an amount to deposit into the account.
                ?? Enter '?' for help.
                <- Enter 'x' to exit.
                """);

            // Display any previous error messages
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(invalidPrompt.ToString());
            Console.ResetColor();
            invalidPrompt.Clear();

            // Ask for input
            Console.Write("Amount: ");

            // Save the current cursor position
            int currentLeft = Console.CursorLeft;
            int currentTop = Console.CursorTop;

            // Display help information
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(helpPrompt.ToString());
            Console.ResetColor();
            helpPrompt.Clear();

            // Set cursor to line amount was asked at and read in the users input
            Console.SetCursorPosition(currentLeft, currentTop);
            input = InputUtilities.GetInputWithinTimeLimit();

            if (input.ToLower() == "x") // Exit back to account options
                exit = true;
            else if (input.ToLower() == "?") // Display help information for deposit
            {
                helpPrompt.Append("""


                    -------- Help ---------
                    For a depoist the input:
                    + Must be a number
                    + Must be greater than 0
                    + (ISA Specific) must be less than the yearly deposit limit
                    -----------------------
                    """);
            }

        } while(!ValidateDepositInput(ref amount, input, ref invalidPrompt) && !exit); // Repeat loop until the deposit amount is valid

        AddToBalance(amount, TransactionType.Deposit); // Add the validated deposit amount to the account balance
    }

    // Withdraw method allows the user to withdraw funds from the account.
    // The user is prompted to enter the withdrawal amount, and the amount is validated.
    // If the amount is valid and sufficient funds are available, it is deducted from the account balance.
    protected void Withdraw()
    {
        // Check if there are sufficient funds in the account before proceeding with the withdrawal
        if (!CheckSufficientFunds()) { return; }

        // Initialize variables to store user input and error messages
        string? input;
        decimal amount = 0;
        StringBuilder invalidPrompt = new StringBuilder();
        StringBuilder helpPrompt = new StringBuilder();

        bool exit = false;
        // Loop until a valid withdrawal amount is entered
        do
        {
            // Display account details and withdraw header
            Console.Clear();
            DisplayAccountDetails();
            Console.WriteLine("""

                ------- Withdraw ------
                ## Provide an amount to withdraw from the account.
                ?? Enter '?' for help.
                <- Enter 'x' to exit.
                """);

            // Display any previous error messages
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(invalidPrompt.ToString());
            Console.ResetColor();
            invalidPrompt.Clear();

            // Ask for input
            Console.Write("Amount: ");

            // Save the current cursor position
            int currentLeft = Console.CursorLeft;
            int currentTop = Console.CursorTop;

            // Display help information
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(helpPrompt.ToString());
            Console.ResetColor();
            helpPrompt.Clear();

            // Set cursor to line amount was asked at and read in the users input
            Console.SetCursorPosition(currentLeft, currentTop);
            input = InputUtilities.GetInputWithinTimeLimit();

            if (input.ToLower() == "x") // Exit back to account options
                exit = true;
            else if (input.ToLower() == "?") // Display help information for deposit
                helpPrompt.Append("""


                    -------- Help ---------
                    For a withdrawal the input:
                    + Must be a number
                    + Must be greater than 0
                    + Account must have sufficient funds including overdraft in some cases
                    -----------------------
                    """);

        } while (!ValidateWithdrawInput(ref amount, input, ref invalidPrompt) && !exit); // Repeat loop until the withdrawal amount is valid

        // If the account implements deposit limit functionality, update the deposit limit
        if (this is IDepositLimitedAccount depositLimitedAccount)
            depositLimitedAccount.UpdateDepositLimit(-amount);

        DeductFromBalance(amount, TransactionType.Withdraw); // Deduct the validated withdrawal amount from the account balance
    }

    // Payment method allows the user to make a payment from the account to another account.
    // The user is prompted to enter the payment amount, and the amount is validated.
    // If the amount is valid and sufficient funds are available, the payment is processed.
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

        bool exit = false;

        // Loop until a valid payee account is selected and a valid payment amount is entered
        do
        {
            // Get payee details (sort code and account number)
            TransactionUtilities.GetPayeeDetails(out string sortCode, out string accountNumber, invalidAccountNumbers, ref exit);

            if (exit) { return; }

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

        } while (payeeAccount == null && !exit);

        StringBuilder helpPrompt = new StringBuilder();
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
            ## Provide an amount for the payment.
            ?? Enter '?' for help.
            <- Enter 'x' to exit.
            """);

            // Display any previous error messages
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(invalidPrompt.ToString());
            Console.ResetColor();
            invalidPrompt.Clear();

            // Ask for input
            Console.Write("Amount: ");
            // Save the current cursor position
            int currentLeft = Console.CursorLeft;
            int currentTop = Console.CursorTop;

            // Display help information
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(helpPrompt.ToString());
            Console.ResetColor();
            helpPrompt.Clear();

            Console.SetCursorPosition(currentLeft, currentTop);
            input = InputUtilities.GetInputWithinTimeLimit();

            if (input.ToLower() == "x")
                return;
            else if (input.ToLower() == "?")
                helpPrompt.Append("""


                    -------- Help ---------
                    For a payment the input:
                    + Must be a number
                    + Must be greater than 0
                    + Account must have sufficient funds including overdraft in some cases
                    + Cannot make a payment to an ISA account. Use transfer instead
                    -----------------------
                    """);

        } while (!ValidateWithdrawInput(ref amount, input, ref invalidPrompt) || !payeeAccount.ValidateDepositInput(ref amount, input, ref invalidPrompt) && !exit); // Repeat loop until both withdrawal and deposit validations pass

        // Deduct the payment amount from the sender's account
        this.DeductFromBalance(amount, TransactionType.Payment);

        Console.WriteLine();
        // Add the payment amount to the payee's account
        payeeAccount.AddToBalance(amount, TransactionType.Payment);

        // Display payment success message
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Payment successful!");
        Console.ResetColor();
        Thread.Sleep(1000);
        
    }

    // Transfer method allows the user to transfer funds from this account to another of the customers account.
    protected void Transfer()
    {
        // Get customer accounts
        List<string> accountNumbers = CustomerReference.ListOfAccounts;
        // Exclude the current account from the list of accounts to transfer to
        accountNumbers.Remove(AccountNumber);

        // Initialize variables
        List<Account> accountObjects = new List<Account>(); // List to store Account objects corresponding to account numbers
        StringBuilder invalidOptionPrompt = new StringBuilder(); // String builder to store error messages

        bool exit = false;
        while (!exit)
        {
            // Prompt user for account selection optiob
            Console.Clear();
            Console.WriteLine("""
            --- Transfer: Account Selection ---
            ## Please enter the ID or account number to select.
            <- Enter 'x' to exit. 
            """);

            // Display any previous error messages
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(invalidOptionPrompt);
            invalidOptionPrompt.Clear();
            Console.ResetColor();

            Console.Write("Enter: ");
            // Save the current cursor position
            int currentLeft = Console.CursorLeft;
            int currentTop = Console.CursorTop;

            // Display the list of available accounts for transfer
            Console.WriteLine("\n\n====== Account(s) =====");
            int count = 1;
            foreach (string accountNumber in accountNumbers)
            {
                // Load and display account details for each account number
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
            string? input = InputUtilities.GetInputWithinTimeLimit();

            int id;
            bool validID = int.TryParse(input, out id);
            if (input.ToLower() == "x")
            {
                exit = true; // Exit the loop if the user chooses to exit
            } 
            else if (validID && id > 0 && id <= accountNumbers.Count)
            {
                TransferAmount(accountNumbers[id-1]); // Transfer amount if the user selects an account by ID
            }
            else if(accountNumbers.Contains(input))
            {
                TransferAmount(input); // Transfer amount if the user selects an account by account number
            }
            else
            {
                invalidOptionPrompt.Append("!!! Invalid ID !!!"); // Display error message for invalid selection
            }
        }
    }

    // TransferAmount method facilitates the transfer of funds from this account to another account.
    // It prompts the user to enter the transfer amount and validates it.
    // If the amount is valid, it deducts the amount from this account and adds it to the recipient account.
    private void TransferAmount(string accountNumber)
    {
        // Load the recipient account details based on the provided account number
        Account TransferToAccount = AccountUtilities.LoadAccountDetails($"{accountNumber}", CustomerReference);

        decimal amount =0; // Initialize the transfer amount

        StringBuilder invalidPrompt = new StringBuilder(); // String builder to store error messages
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
            input = InputUtilities.GetInputWithinTimeLimit();

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
        Thread.Sleep(1000); // Pause briefly to display the success message
    }

    // Deducts specified amount from balance and logs transaction.
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

    // Adds specified amount to balance and logs transaction.
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

    // Checks if the account has sufficient funds for the requested transaction.
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

    // Validates withdrawal input.
    protected bool ValidateWithdrawInput(ref decimal amount, string? input, ref StringBuilder invalidPrompt)
    {
        if (input == "x" || input == "?")
        {
            return false;
        }
        else if (!decimal.TryParse(input, out amount)) // Validate input is a decimal number
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

    // Validates deposit input.
    protected bool ValidateDepositInput(ref decimal amount, string? input, ref StringBuilder invalidPrompt)
    {
        if (input == "x" || input == "?")
        {
            return false;
        }
        else if (!decimal.TryParse(input, out amount)) // Validate input is a decimal number
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
