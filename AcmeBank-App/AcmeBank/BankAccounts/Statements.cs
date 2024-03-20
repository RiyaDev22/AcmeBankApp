using AcmeBank.BankAccounts.Transactions;
using Microsoft.VisualBasic;
using System.Text;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AcmeBank.BankAccounts;

internal class Statements
{
    

    internal static void StatementOptions(string accountNumber)
    {
        DateOnly date;
        //Ask for a month and year
        bool initialPass = true;
        string? input;
        do
        {
            // Display a header for statement options with a description
            Console.Clear();
            Console.WriteLine("""
            -- Statement options --
            Enter a month and year
            in the format (MM-YYYY)
            """);

            // Provides a prompt for invalid inputs
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(initialPass? "" : "!!! Invalid Input must be in the form (MM-YYYY) !!!");
            Console.ResetColor();
            initialPass = false;

            // Takes an input from the user
            Console.Write("Enter: ");
            input = Console.ReadLine();

        } while (!DateOnly.TryParse(input, out date));//Validate input

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

        Console.WriteLine($"""
            ---------------------- Statement {date.Month,2}/{date.Year,4} ---------------------- 
                     Amount         Balance            Type            Date
            {monthlyStatement}
            ---------------------------------------------------------------
            """);

        Console.ReadLine();
        // Then provide the option to send statement or exit
    }
}
