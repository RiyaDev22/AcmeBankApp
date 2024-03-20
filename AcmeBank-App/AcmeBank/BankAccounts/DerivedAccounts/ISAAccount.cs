using AcmeBank.BankAccounts;
using AcmeBank.BankAccounts.AccountInterfaces;
using AcmeBank.BankAccounts.Transactions;
using System.Collections.Generic;
using System.Net;
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
    public ISAAccount(string accountNumber, string sortCode, decimal balance, string address) : base(accountNumber, sortCode, balance, AccountType.ISA, address)
    {
        _remainingDepositLimit = DepositLimit;
    }

    // Used on loading account from file
    public ISAAccount(string accountNumber, string sortCode, decimal balance, string address, decimal remainingDepositLimit) : base(accountNumber, sortCode, balance, AccountType.ISA, address)
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
            Interest Rate: {_InterestRate:P2}
            -----------------------

            """);
    }

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

    public void ResetDepositLimit()
    {
        // Reset the remaining deposit limit to the deposit limit
        RemainingDepositLimit = DepositLimit;
    }

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

    protected override void DisplayAccountOptions()
    {
        Console.WriteLine("""
                --- Account options ---
                1. Deposit
                2. Withdraw
                3. Transfer
                4. Calculate interest (Test)
                5. Statement
                X. Exit
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
                CalculateAndApplyInterest();
                break;
            case "x":
                // Exit the loop if the user chooses to exit
                return true;
            case "5":
                Statements.StatementOptions(AccountNumber);
                break;
            default:
                // Display an error message if the user enters an invalid option
                Console.Clear();
                invalidPrompt.Append("-- !!! Invalid option !!! --");
                break;
        }
        return false; //does not exit the loop
    }

    private void CalculateAndApplyInterest()
    {
        // Load transaction history
        List<Transaction>transactionHistory = TransactionUtilities.LoadTransactionHistory(AccountNumber);
        if (transactionHistory.Count == 0) { return; }

        transactionHistory.Reverse();

        DateTime currentDate = DateTime.Now;
        DateTime targetDate = currentDate.AddDays(-365);

        // Dictionary to store the latest transaction for each day
        HashSet<DateTime> latestTransactions = new HashSet<DateTime>();
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

        order.Reverse();
        //previous balance
        // get the previous balance then the days up till the next from the last
        TimeSpan gap;
        int daysGap;
        int daysSum = 0;

        decimal yearlBalanceSum = 0;

        DateTime previousDate = targetDate;
        foreach (var index in order)
        {
            gap = transactionHistory[index].Date - previousDate.Date;
            daysGap = Math.Abs((int)gap.TotalDays);
            daysSum += daysGap;

            yearlBalanceSum += previousBalance * daysGap;
            previousBalance = transactionHistory[index].Balance;

            previousDate = transactionHistory[index].Date;
        }
        gap = currentDate - previousDate.Date;
        daysGap = Math.Abs((int)gap.TotalDays);
        daysSum += daysGap;

        yearlBalanceSum += previousBalance * daysGap;

        decimal interestGained = (yearlBalanceSum / daysSum) * 0.0275m;
        AddToBalance(interestGained, TransactionType.Interest);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"You have recieved: {interestGained:C2} in interest");
        Console.ResetColor();
        Thread.Sleep(1500); // Pauses for 1.5seconds
    }
    #endregion

}
