using BankPayments.BankAccounts.DerivedAccounts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Xml.Linq;

namespace AcmeBank.BankAccounts.Transactions;

internal class TransactionUtilities
{
    private static string directory = $@"{AppDomain.CurrentDomain.BaseDirectory}\Accounts\"; // directory for accounts file

    // Checks sort code is 6 numeric digits
    internal static bool VerifySortCode(string sortCode)
    {
        // Define regex pattern for 6-digit numeric string
        string pattern = @"^\d{6}$";
        return Regex.IsMatch(sortCode, pattern);
    }

    // Checks account number is 8 numeric digits
    internal static bool VerifyAccountNumber(string accountNumber)
    {
        // Define regex pattern for 8-digit numeric string
        string pattern = @"^\d{8}$";
        return Regex.IsMatch(accountNumber, pattern);
    }

    // Method to get payee details such as sort code and account number for making a payment.
    internal static void GetPayeeDetails(out string sortCode, out string accountNumber, List<string> invalidAccountNumbers, ref bool exit)
    {
        sortCode = ""; // Initialize sort code
        accountNumber = ""; // Initialize account number

        StringBuilder invalidPrompt = new StringBuilder();
        StringBuilder helpPrompt = new StringBuilder();

        bool validInputs = false; // Flag to indicate if inputs are valid
        do
        {
            // Display payee header
            Console.Clear();
            Console.WriteLine("""
                ---- Payee details ----
                ## Please provide the details of the account they want to make a payment to.
                <- Enter x at any point to exit.
                """);

            // Display any error messages
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(invalidPrompt.ToString());
            Console.ResetColor();
            invalidPrompt.Clear();

            // Ask user to input sort code
            Console.Write("""
                Please enter Sort Code. e.g (123456)
                Sort Code: 
                """);
            sortCode = InputUtilities.GetInputWithinTimeLimit();

            if(sortCode.ToLower() == "x") 
            { 
                exit = true;
                return;
            }
            else if(!TransactionUtilities.VerifySortCode(sortCode)) // Checks its 6 digits long
            {
                invalidPrompt.Append("!!! Invalid sort code !!!");
                continue;
            }

            // Ask user to input account number
            Console.Write("""
                Please enter Account Number. e.g (12345678)
                Account Number: 
                """);
            accountNumber = InputUtilities.GetInputWithinTimeLimit();

            if (accountNumber.ToLower() == "x")
            {
                exit = true;
                return; //Exits payment or standing order setup
            } 
            else if (!TransactionUtilities.VerifyAccountNumber(accountNumber)) // Checks its 8 digits long
            {
                invalidPrompt.Append("!!! Invalid account number !!!");
                continue;
            }
            else if (invalidAccountNumbers.Contains(accountNumber)) // Ensures its not to this customers owned account(s)
            {
                invalidPrompt.Append("!!! Payment to own account, Use transfer instead !!!");
                continue;
            } 
            else
            {
                validInputs = true; // Inputs are valid
            }

        } while (!validInputs); // Loops if sort code and account number provided are invalid
    }

    // Loads transaction history from a file based on the provided account number.
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
                    // Split each transaction entry by comma and parse values
                    string[] transactionSplit = transaction.Split(',');
                    decimal amount = decimal.Parse(transactionSplit[0]);
                    decimal balance = decimal.Parse(transactionSplit[1]);
                    TransactionType type = (TransactionType)Enum.Parse(typeof(TransactionType), transactionSplit[2]);
                    DateTime date = DateTime.Parse(transactionSplit[3]);

                    // Create a new Transaction object and add it to the transaction history list
                    transactionHistory.Add(new Transaction(amount, balance, type, date));
                }
                return transactionHistory; // Return the transaction history list
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

        return transactionHistory; // Return an empty list if file doesn't exist or an error occurs
    }
}

