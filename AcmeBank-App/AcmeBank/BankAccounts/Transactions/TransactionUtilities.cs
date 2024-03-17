using System.Text;
using System.Text.RegularExpressions;

namespace AcmeBank.BankAccounts.Transactions;

internal class TransactionUtilities
{
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
}
