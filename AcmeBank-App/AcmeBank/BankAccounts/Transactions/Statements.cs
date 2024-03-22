using System.Text;

namespace AcmeBank.BankAccounts.Transactions;

internal class Statements
{
    private static Customer _currentCustomer { get; set; }
    public static void StatementOptions(string accountNumber, Customer customer)
    {
        _currentCustomer = customer;

        DateOnly date = DateOnly.MinValue;
        StringBuilder dateStatement = new StringBuilder();
        StringBuilder invalidPrompt = new StringBuilder();

        bool exit = false;

        bool validInput = false;
        while (!validInput)
        {
            date = RetrieveDateYear(invalidPrompt, ref exit);
            if (exit) { return; };
            invalidPrompt.Clear();

            dateStatement = GetDateTransactions(accountNumber, date);


            if (dateStatement.ToString() == "")
                invalidPrompt.Append($"!!! No transactions during {date.Month:D2}/{date.Year:D4} !!!");
            else
                validInput = true;
        }

        DisplayAndRequestStatement(dateStatement, date, accountNumber);
    }

    static DateOnly RetrieveDateYear(StringBuilder invalidPrompt, ref bool exit)
    {
        //Ask for a month and year
        DateOnly date;
        bool initialPass = true;
        string? input;
        do
        {
            // Display a header for statement options with a description
            Console.Clear();
            Console.WriteLine("""
            -- Statement options --
            ## Enter a month and year.
            ## in the format (MM-YYYY).
            <- Enter x to exit.
            """);

            // Provides a prompt for invalid inputs
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(invalidPrompt);
            invalidPrompt.Clear();
            Console.ResetColor();

            // Takes an input from the user
            Console.Write("Month-Year: ");
            input = Console.ReadLine();

            if (input.ToLower() == "x")
                exit = true;
            else
                invalidPrompt.Append("!!! Invalid Input must be in the form (MM-YYYY) !!!");


        } while (!DateOnly.TryParse(input, out date) && !exit);//Validate input

        return date;
    }

    static StringBuilder GetDateTransactions(string accountNumber, DateOnly date)
    {
        StringBuilder monthlyStatement = new StringBuilder();
        // Should create and display the statement
        List<Transaction> transactionHistory = TransactionUtilities.LoadTransactionHistory(accountNumber);
        foreach (Transaction transaction in transactionHistory)
        {
            // Check if the transaction's date falls within the same month and year as the entered date
            if (transaction.Date.Year == date.Year && transaction.Date.Month == date.Month)
            {
                monthlyStatement.AppendLine($"{transaction.Amount,15:C2} {transaction.Balance,15:C2} {transaction.Type,15} {transaction.Date,15:d}");
            }
        }
        return monthlyStatement;
    }

    static void DisplayAndRequestStatement(StringBuilder dateStatement, DateOnly date, string accountNumber)
    {

        Console.Clear();
        Console.Write("""
            -- Statement options --
            ## Would you like to send the statement to the customer?.
            ## Enter 'y' for yes.
            <- Enter any other key to exit.
            
            Your choice: 
            """);

        // Save the current cursor position
        int currentLeft = Console.CursorLeft;
        int currentTop = Console.CursorTop;


        Console.WriteLine($"""
            

            ---------------------- Statement {date.Month:D2}/{date.Year:D4} ---------------------- 
                     Amount         Balance            Type            Date
            {dateStatement}
            ---------------------------------------------------------------
            """);

        // Move the cursor position to the line where user input is expected
        Console.SetCursorPosition(currentLeft, currentTop);

        // Then provide the option to send statement or exit
        string? input = Console.ReadLine();
        if (input.ToLower() == "y")
        {
            Account account = AccountUtilities.LoadAccountDetails(accountNumber,_currentCustomer);
            Console.SetCursorPosition(0, currentTop - 1);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"Statement sent to {account.Address}");
            Console.ResetColor();

            Thread.Sleep(2000); // Pause for 2 seconds
        }
    }
}
