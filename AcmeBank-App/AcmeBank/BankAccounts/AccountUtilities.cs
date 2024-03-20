using AcmeBank.BankAccounts.AccountInterfaces;
using AcmeBank.BankAccounts.Transactions;

using System.Text;
using BankPayments.BankAccounts.DerivedAccounts;

namespace AcmeBank.BankAccounts;

internal class AccountUtilities
{
    private static string directory = $@"{AppDomain.CurrentDomain.BaseDirectory}\Accounts\";

    internal static void SaveAccountDetails(Account accountToSave)
    {
        string fileDirectory = $@"{directory}\{accountToSave.AccountNumber}"; // Construct the file directory path

        // Check if the directory exists, if not, create it
        if (!Directory.Exists(fileDirectory))
        {
            Directory.CreateDirectory(fileDirectory);
            using (File.Create($@"{fileDirectory}\AccountDetails.csv")) ;
            using (File.Create($@"{fileDirectory}\Transaction.csv")) ;
        }

        // Construct the path for AccountDetails.csv
        string path = @$"{fileDirectory}\AccountDetails.csv";

        // Create a StringBuilder to construct the CSV content
        StringBuilder sb = new StringBuilder();
        sb.Append($"{accountToSave.AccountNumber},");
        sb.Append($"{accountToSave.SortCode},");
        sb.Append($"{accountToSave.Balance},");
        sb.Append($"{accountToSave.Type},");
        sb.Append($"{accountToSave.Address},");

        // Append additional details based on the account type
        switch (accountToSave.Type)
        {
            case AccountType.Personal:
                IOverdraftAccount personalAccount = accountToSave as IOverdraftAccount;
                if (personalAccount != null)
                {
                    decimal overdraftRemaining = personalAccount.OverdraftRemaining;
                    sb.Append($"{overdraftRemaining},"); // Append remaining overdraft limit to the CSV
                }
                break;
            case AccountType.ISA:
                IDepositLimitedAccount isaAccount = accountToSave as IDepositLimitedAccount;
                if (isaAccount != null)
                {
                    decimal remainingLimit = isaAccount.RemainingDepositLimit;
                    sb.Append($"{remainingLimit},"); // Append remaining deposit limit to the CSV
                }
                break;
            case AccountType.Business:
                IOverdraftAccount businessAccount = accountToSave as IOverdraftAccount;
                if (businessAccount != null)
                {
                    decimal overdraftRemaining = businessAccount.OverdraftRemaining;
                    sb.Append($"{overdraftRemaining},"); // Append remaining overdraft limit to the CSV
                }
                break;
            default:
                break;
        }
        // Write the CSV content to the file
        File.WriteAllText(path, sb.ToString());

        //Console.ForegroundColor = ConsoleColor.Green;
        //Console.WriteLine("Account successfully saved!");
        //Console.ResetColor();

        //Thread.Sleep(1000);
    }

    internal static Account LoadAccountDetails(string accountNumberToLoad)
    {
        // Construct the file directory path
        string fileDirectory = $@"{directory}\{accountNumberToLoad}";
        string path = $@"{fileDirectory}\AccountDetails.csv";

        Account account = null; // Initialized account will return null if any issues in loading from file

        try
        {
            if (File.Exists(path))
            {
                // Read the file
                string[] accountDetailsString = File.ReadAllLines(path);
                string[] accountSplit = accountDetailsString[0].Split(','); // Should never have more than 1 line
                string accountNumber = accountSplit[0];
                string sortCode = accountSplit[1];
                decimal balance = decimal.Parse(accountSplit[2]);
                string address = accountSplit[4];
                switch (accountSplit[3])
                {
                    case "Personal":
                        decimal remainingOverdraftPA = decimal.Parse(accountSplit[5]);
                        account = new PersonalAccount(accountNumber, sortCode, balance, address, remainingOverdraftPA);
                        break;
                    case "ISA":
                        decimal remainingDepositLimit = decimal.Parse(accountSplit[5]);
                        account = new ISAAccount(accountNumber, sortCode, balance, address,remainingDepositLimit);
                        break ;
                    case "Business":
                        decimal remainingOverdraftBA = decimal.Parse(accountSplit[5]);
                        account = new BusinessAccount(accountNumber, sortCode, balance, address, remainingOverdraftBA);
                        break;
                    default:
                        return null;
                }
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

        return account;
    }

    internal static void SaveTransaction(Transaction transaction, string accountNumber)
    {
        // Construct the file directory path
        string fileDirectory = $@"{directory}\{accountNumber}";

        // Create the directory if it doesn't exist
        if (!Directory.Exists(fileDirectory))
        {
            Directory.CreateDirectory(fileDirectory);
            //using (File.Create($@"{fileDirectory}\AccountDetails.csv")) ;
            using (File.Create($@"{fileDirectory}\Transaction.csv"));
        }

        // Construct the file path
        string path = @$"{fileDirectory}\Transaction.csv";

        // Construct the string representation of the transaction
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"{transaction.Amount},{transaction.Balance},{transaction.Type},{transaction.Date}");

        // Append the transaction details to the CSV file
        File.AppendAllText(path, sb.ToString());

        //Console.ForegroundColor = ConsoleColor.Green;
        //Console.WriteLine($"{transaction.Type} successfully saved!");
        //Console.ResetColor();

        //Thread.Sleep(1000);
    }
}
