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
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(invalidPrompt.ToString());
            Console.ResetColor();
            invalidPrompt.Clear();

            // Ask user to input sort code
            Console.Write("""
                Please enter Sort Code. e.g (123456)
                Sort Code: 
                """);
            sortCode = Console.ReadLine();

            // Ask user to input account number
            Console.Write("""
                Please enter Account Number. e.g (12345678)
                Account Number: 
                """);
            accountNumber = Console.ReadLine();

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

                transactionHistory.Reverse();

                DateTime currentDate = DateTime.Now;
                DateTime targetDate = currentDate.AddDays(-365);

                // Dictionary to store the latest transaction for each day
                HashSet<DateTime> latestTransactions = new HashSet<DateTime>();
                List<int> order = new List<int>();
                // Loop through transaction history and filter transactions between targetDate and currentDate
                bool hasPreviousBalance = false;
                decimal previousBalance = 0;
                for(int i = 0; i < transactionHistory.Count; i++)
                {
                    // Check if the transaction date is between targetDate and currentDate
                    if (transactionHistory[i].Date >= targetDate && transactionHistory[i].Date <= currentDate && !latestTransactions.Contains(transactionHistory[i].Date.Date))
                    {
                        latestTransactions.Add(transactionHistory[i].Date);
                        order.Add(i);
                        Console.WriteLine($"{transactionHistory[i].Date.Date}, {transactionHistory[i].Balance}");

                    }
                    else if (transactionHistory[i].Date < targetDate && !latestTransactions.Contains(transactionHistory[i].Date.Date) && !hasPreviousBalance)
                    {
                        previousBalance = transactionHistory[i].Balance;
                        hasPreviousBalance = true;
                    }
                }

                order.Reverse();
                Console.WriteLine();


                //previous balance
                // get the previous balance then the days up till the next from the last
                TimeSpan gap;
                int daysGap;
                int daysSum = 0;

                decimal yearlBalanceSum = 0;
                Console.WriteLine(previousBalance);

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

                Console.WriteLine($"sum of days: {daysSum}, average balance: {yearlBalanceSum/365.00m:C}, interest: {((yearlBalanceSum / 365.00m) * 0.0275m):C}");

                Console.ReadLine();

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
            Thread.Sleep(1000);
        }

        return transactionHistory;
    }

}
