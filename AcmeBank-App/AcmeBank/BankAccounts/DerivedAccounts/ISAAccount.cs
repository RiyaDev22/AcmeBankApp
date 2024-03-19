using AcmeBank.BankAccounts;
using AcmeBank.BankAccounts.AccountInterfaces;
using AcmeBank.BankAccounts.Transactions;
using System.Collections.Generic;
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
    public ISAAccount(string accountNumber, string sortCode, decimal balance) : base(accountNumber, sortCode, balance, AccountType.ISA)
    {
        _remainingDepositLimit = DepositLimit;
    }

    // Used on loading account from file
    public ISAAccount(string accountNumber, string sortCode, decimal balance, decimal remainingDepositLimit) : base(accountNumber, sortCode, balance, AccountType.ISA)
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
                CalculateInterest();
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

    private void CalculateInterest()
    {
        // Load transaction history
        TransactionUtilities.LoadTransactionHistory(AccountNumber);
        // Collect all the last balances of each day.
        // Use this to calculate an average
        // Divide by 365 and multiple by 2.75%
        Console.ReadLine();
    }
    #endregion

}
