using AcmeBank;
using AcmeBank.BankAccounts;
using AcmeBank.BankAccounts.AccountInterfaces;
using AcmeBank.BankAccounts.Transactions;
using System.Text;

namespace BankPayments.BankAccounts.DerivedAccounts;

public class ISAAccount : Account, IDepositLimitedAccount
{
    #region Attributes
    private const decimal _yearlyDepositLimit = 20_000m;
    private decimal _remainingDepositLimit; //store remaining deposit limit
    private const decimal _InterestRate = 0.0275m;
    #endregion

    #region Constructors
    // Used on account creation
    public ISAAccount(string accountNumber, string sortCode, decimal balance, string address, Customer customer) : base(accountNumber, sortCode, balance, AccountType.ISA, address, customer)
    {
        _remainingDepositLimit = DepositLimit;
    }

    // Used on loading account from file
    public ISAAccount(string accountNumber, string sortCode, decimal balance, string address, decimal remainingDepositLimit, Customer customer) : base(accountNumber, sortCode, balance, AccountType.ISA, address, customer)
    {
        _remainingDepositLimit = remainingDepositLimit;
    }


    #endregion

    #region Getters/Setters
    public decimal DepositLimit { get { return _yearlyDepositLimit; } } // £20,000 deposit limit for ISA account

    public decimal RemainingDepositLimit
    {
        get => _remainingDepositLimit;
        set
        {
            // Ensure the new value is clamped between 0 and the deposit limit
            _remainingDepositLimit = Math.Clamp(value, 0 , DepositLimit); // Used when withdrawing money.
        }
    }

    #endregion

    #region Methods

    // Diplsays the account details including the deposit limit
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
            Interest Rate: {_InterestRate:P2}
            -----------------------
            """);
    }

    // Update the remaining deposit limt
    public bool UpdateDepositLimit(decimal amount)
    {
        // Check if the deposit amount exceeds the remaining deposit limit
        if (RemainingDepositLimit - amount < 0)
        { 
            return false;
        }
        else
        {
            // Deduct the deposit amount from the remaining deposit limit
            RemainingDepositLimit -= amount;
            return true;
        }
    }

    // Resets the deposit limit
    public void ResetDepositLimit()
    {
        // Reset the remaining deposit limit to the deposit limit
        RemainingDepositLimit = DepositLimit;
    }

    // Extends the base deposit so that we dont exceed the yearly deposit
    protected override void Deposit()
    {
        if (RemainingDepositLimit > 0)
        {
            base.Deposit();
        } 
        else
        {
            // Display error prompt if deposit is over the yearly limit
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("!!! Yearly deposit limit of £20,000 reached !!!");
            Console.ResetColor();

            // Provide a pause to read prompt 1.5seconds
            Thread.Sleep(1500);
            Console.Clear();
        }
    }

    // Display the options for the account including the interest forecast
    protected override void DisplayAccountOptions()
    {
        Console.WriteLine("""

                --- Account options ---
                1. Deposit
                2. Withdraw
                3. Transfer
                4. Calculate Interest
                5. Statement

                x. Exit
                -----------------------
                """);
    }

    // Receives an input from the menu loop. The input
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
                Transfer();
                break;
            case "4":
                CalculateInterest();
                break;
            case "x":
                // Exit the loop if the user chooses to exit
                return true;
            case "5":
                Statements.StatementOptions(AccountNumber, CustomerReference);
                break;
            default:
                // Display an error message if the user enters an invalid option
                Console.Clear();
                invalidPrompt.Append("-- !!! Invalid option !!! --");
                break;
        }
        return false; //does not exit the loop
    }

    // Calculates the interest gained over the past year based on transaction history.
    private void CalculateInterest()
    {
        // Load transaction history
        List<Transaction>transactionHistory = TransactionUtilities.LoadTransactionHistory(AccountNumber);
        if (transactionHistory.Count == 0) { return; }

        transactionHistory.Reverse();

        DateTime currentDate = DateTime.Now;
        DateTime targetDate = currentDate.AddDays(-365);

        // Dictionary to store the latest transaction for each day
        List<DateTime> latestTransactions = new List<DateTime>();
        List<int> order = new List<int>();

        // Loop through transaction history and filter transactions between targetDate and currentDate
        bool hasPreviousBalance = false;
        decimal previousBalance = 0;
        for (int i = 0; i < transactionHistory.Count; i++)
        {
            // Check if the transaction date is between targetDate and currentDate
            if (transactionHistory[i].Date >= targetDate && transactionHistory[i].Date <= currentDate && !latestTransactions.Contains(transactionHistory[i].Date.Date))
            {
                latestTransactions.Add(transactionHistory[i].Date);
                order.Add(i);

            } else if (transactionHistory[i].Date < targetDate && !latestTransactions.Contains(transactionHistory[i].Date.Date) && !hasPreviousBalance)
            {
                previousBalance = transactionHistory[i].Balance;
                hasPreviousBalance = true;
            }
        }
        // Reverse the order list to process transactions from past to present
        order.Reverse();

        // Calculate interest based on daily balances
        TimeSpan gap;
        int daysGap;
        int daysSum = 0;

        decimal yearlBalanceSum = 0;

        // Loops through the transaction history
        DateTime previousDate = targetDate;
        foreach (var index in order)
        {
            // Determines an interger daysGap from the gap in days between the transactions
            gap = transactionHistory[index].Date - previousDate.Date;
            daysGap = Math.Abs((int)gap.TotalDays);
            daysSum += daysGap;

            // This gap is used to determine how long the balance stayed the same across those dates
            yearlBalanceSum += previousBalance * daysGap;
            previousBalance = transactionHistory[index].Balance;

            // This sets the previous date allowing us to find the next gap
            previousDate = transactionHistory[index].Date;
        }
        // Finally we need the gap from the current date to the last transaction
        gap = currentDate - previousDate.Date;
        daysGap = Math.Abs((int)gap.TotalDays);
        daysSum += daysGap;

        // Again we find out and add how long the balance has stayed the same for
        yearlBalanceSum += previousBalance * daysGap;

        // Calculate interest gained
        decimal interestGained = (yearlBalanceSum / daysSum) * 0.0275m;

        // Output the calculated interest
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"""

            From {targetDate:D} -> {currentDate.Date:D}
            You have gained {interestGained:C2} in interest
            """);
        Console.ResetColor();
        Thread.Sleep(4000); // Pauses for 4 seconds
    }
    #endregion

}
