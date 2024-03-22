using BankPayments.BankAccounts.DerivedAccounts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;

namespace AcmeBank.BankAccounts.Transactions;

internal class TransactionUtilities
{
    private static string directory = $@"{AppDomain.CurrentDomain.BaseDirectory}\Accounts\";

    internal static bool VerifySortCode(string sortCode)
    {
        // Define regex pattern for 6-digit numeric string
        string pattern = @"^\d{6}$";
        return Regex.IsMatch(sortCode, pattern);
    }

    internal static bool VerifyAccountNumber(string accountNumber)
    {
        // Define regex pattern for 8-digit numeric string
        string pattern = @"^\d{8}$";
        return Regex.IsMatch(accountNumber, pattern);
    }

    internal static void GetPayeeDetails(out string sortCode, out string accountNumber, List<string> invalidAccountNumbers)
    {
        StringBuilder invalidPrompt = new StringBuilder();
        bool validInputs = false;
        do
        {
            // Display payee header
            Console.Clear();
            Console.WriteLine("""
                ---- Payee details ----
                """);

            // Display any error messages
            if(invalidPrompt.ToString() != "")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(invalidPrompt.ToString());
                Console.ResetColor();
                invalidPrompt.Clear();
            }

            // Ask user to input sort code
            Console.Write("""
                Please enter Sort Code. e.g (123456)
                Sort Code: 
                """);
            sortCode = InputUtilities.GetInputWithinTimeLimit();

            // Ask user to input account number
            Console.Write("""
                Please enter Account Number. e.g (12345678)
                Account Number: 
                """);
            accountNumber = InputUtilities.GetInputWithinTimeLimit();

            // Check if both sort code and account number are valid
            if (invalidAccountNumbers.Contains(accountNumber))
                invalidPrompt.Append("!!! Payment to own account, Use transfer instead !!!");
            else if (TransactionUtilities.VerifySortCode(sortCode) && TransactionUtilities.VerifyAccountNumber(accountNumber))
                validInputs = true;
            else
                invalidPrompt.Append("!!! Invalid inputs !!!");

        } while (!validInputs); // Loops if sort code and account number provided are invalid
    }


    internal static List<Transaction> LoadTransactionHistory(string accountNumberToLoad)
    {
        // Construct the file directory path
        string fileDirectory = $@"{directory}\{accountNumberToLoad}";
        string path = $@"{fileDirectory}\Transaction.csv";

        List<Transaction> transactionHistory = new List<Transaction>();

        try
        {
            if (File.Exists(path))
            {
                // Read the file
                string[] transactions = File.ReadAllLines(path);
                foreach (string transaction in transactions)
                {
                    string[] transactionSplit = transaction.Split(',');
                    decimal amount = decimal.Parse(transactionSplit[0]);
                    decimal balance = decimal.Parse(transactionSplit[1]);
                    TransactionType type = (TransactionType)Enum.Parse(typeof(TransactionType), transactionSplit[2]);
                    DateTime date = DateTime.Parse(transactionSplit[3]);
                    transactionHistory.Add(new Transaction(amount, balance, type, date));
                }
                return transactionHistory;
            }
        } catch (IndexOutOfRangeException)
        {
            // Handle parsing errors
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Something went wrong parsing the file, please check the data!");

        } catch (FileNotFoundException)
        {
            // Handle file not found error
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("The file couldn't be found!");

        } catch (Exception)
        {
            // Handle other exceptions
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Something went wrong while loading the file!");

        } finally
        {
            // Reset console color and provide a delay for user to see the message
            Console.ResetColor();
        }

        return transactionHistory;
    }

}
