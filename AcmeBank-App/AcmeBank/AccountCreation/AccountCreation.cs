using AcmeBank.BankAccounts;
using BankPayments.BankAccounts.DerivedAccounts;
using System.Numerics;

namespace AcmeBank;
public class AccountCreation
{
    private static string accountCsvFile = "accounts.csv"; // Path to the CSV file
    // DisplayMenu Method
    public static void DisplayMenu(Customer customer)
    {
        bool menuRunning = true;
        while (menuRunning)
        {
            Console.Write("""
                ============================
                    **ACCOUNT CREATION** 
                ============================
                1. Personal Account.
                2. Individual Savings Account (ISA).
                3. Business Account.
                ============================
                Please, Select Account Type or enter 'X' to exit: 
                """);
            string? choice = InputUtilities.GetInputWithinTimeLimit().ToLower();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("\nCreating Personal Account...");
                    // Call personal account creation method
                    CreatePersonalAccount(customer);
                    break;
                case "2":
                    Console.WriteLine("\nCreating Individual Savings Account (ISA)...");
                    // Call ISA account creation method
                    CreateISA(customer);
                    break;
                case "3":
                    Console.WriteLine("\nCreating Business Account...");
                    // Call business account creation method
                    CreateBusinessAccount(customer);
                    break;
                case "x":
                    menuRunning = false;
                    break;
                default:
                    Console.WriteLine("\nInvalid choice. Please try again.");
                    break;
            }
        }
    }

    // CreatePersonalAccount Method
    private static void CreatePersonalAccount(Customer customer)
    {
        // Request photo ID until the user confirms identity
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Type 'back' at any prompt to return to the Main Menu.");
        Console.ResetColor(); // Reset the color to the default
        string photoID = GetUserInput("Please, provide photo ID (type 'confirm' to proceed): ");
        if (photoID.ToLower() == "back") // Check if the user wants to go back
        {
            return;
        }

        // Request address ID until the user confirms identity
        string addressID = GetUserInput("Please, provide address ID (type 'confirm' to proceed): ");
        if (addressID.ToLower() == "back") // Check if the user wants to go back
        {
            return;
        }

        // Request initial deposit
        decimal initialDeposit = RequestInitialDeposit(true, false);
        if (initialDeposit == -1) // Check if the user wants to go back
        {
            return;
        }

        // Personal account created auccessfully
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Personal account created successfully!");
        Console.ResetColor(); // Reset the color to the default

        // Generate and save account number
        string personalAccountNumber = GenerateAccountNumber();

        // Generate and save sort code specific to the customer
        string personalSortCode = "123456";

        Account account = new PersonalAccount(personalAccountNumber, personalSortCode, initialDeposit, customer.PostCode, customer);
        AccountUtilities.SaveAccountDetails(account);

        customer.AddAccount(account);

    }

    // CreateISA Method
    private static void CreateISA(Customer customer)
    {
        // Check if the customer already has an ISA account
        if (customer.HasISA)
        {
            Console.WriteLine("Customer already has an ISA account. Account creation canceled.");
            return;
        }

        // Request photo ID
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Type 'back' at any prompt to return to the Main Menu.");
        Console.ResetColor(); // Reset the color to the default
        string photoID = GetUserInput("Please, provide photo ID (type 'confirm' to proceed): ");
        if (photoID.ToLower() == "back") // Check if the user wants to go back
        {
            return;
        }

        // Request address ID
        string addressID = GetUserInput("Please, provide address ID (type 'confirm' to proceed): ");
        if (addressID.ToLower() == "back") // Check if the user wants to go back
        {
            return;
        }

        // Request initial deposit
        decimal initialDeposit = RequestInitialDeposit(true, true); // Minimum and maximum requirements
        if (initialDeposit == -1) // Check if the user wants to go back
        {
            return;
        }

        // ISA account created successfully
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Individual Savings Account (ISA) created successfully!");
        Console.ResetColor(); // Reset the color to the default

        // Generate and save account number
        string ISANumber = GenerateAccountNumber();

        // Generate and save sort code specific to the customer
        string ISASortCode = "123123";

        Account account = new ISAAccount(ISANumber, ISASortCode, initialDeposit, customer.PostCode, customer);
        AccountUtilities.SaveAccountDetails(account);

        customer.AddAccount(account);

    }

    // CreateBusinessAccount Method
    private static void CreateBusinessAccount(Customer customer)
    {
        // List of disallowed business types
        List<string> disallowedBusinessTypes = new List<string>
    {
        "Enterprise",
        "PLC",
        "Charity",
        "Public Sector"
    };

        // Prompt user to select a business type
        string? businessType;
        do
        {
            // Request user input for business type
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Type 'back' at any prompt to return to the Main Menu.");
            Console.ResetColor(); // Reset the color to the default
            Console.Write("Enter the business type: ");
            businessType = InputUtilities.GetInputWithinTimeLimit()?.Trim();

            if (businessType?.ToLower() == "back") // Check if the user wants to go back
            {
                return;
            }

            if (disallowedBusinessTypes.Contains(businessType, StringComparer.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Business type '{businessType}' is not eligible to create a business account.");
                Console.WriteLine("Contact 'Alternative Business Account Department' at '0000 111 2222' for more info!");
                Console.WriteLine("Returning to the Main Menu...");
                return;
            }
        } while (string.IsNullOrEmpty(businessType));

        // Request proof of business existence and details
        string confirmBusinessDetails = GetUserInput("Please, type 'confirm' to prove your business existence and its details: ");
        if (confirmBusinessDetails.ToLower() == "back") // Check if the user wants to go back
        {
            return;
        }

        if (confirmBusinessDetails != "confirm")
        {
            Console.WriteLine("You have not confirmed your business. Please try again!");
            return;
        }

        // Store business details (Not stored anywhere - YET)
        Console.WriteLine("Your business details have been stored successfully!");

        // Request initial deposit with no minimum requirement
        decimal initialDeposit = RequestInitialDeposit(false, false);
        if (initialDeposit == -1) // Check if the user wants to go back
        {
            return;
        }

        // If all the checks pass, then the account is created
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Business account created successfully!");
        Console.ResetColor(); // Reset the color to the default

        // Generate and save account number
        string businessAccountNumber = GenerateAccountNumber();

        // Generate and save sort code specific to the customer
        string businessSortCode = "456456";

        Account account = new BusinessAccount(businessAccountNumber, businessSortCode, initialDeposit, customer.PostCode, customer);
        AccountUtilities.SaveAccountDetails(account);

        customer.AddAccount(account);

    }

    // RequestInitialDeposit Method
    private static decimal RequestInitialDeposit(bool requireMinimum, bool requireMaximum)
    {
        // Prompt message based on requirement
        string promptMessage = requireMinimum ? "Please, provide the initial deposit amount (at least £1): " : "Please, provide the initial deposit amount (enter £0 or greater): ";
        promptMessage += requireMaximum ? " (Maximum £20,000): " : "";

        // Request initial deposit amount
        decimal initialDeposit;
        string? depositInput;
        bool isValidInput = false;

        do
        {
            Console.Write(promptMessage);
            depositInput = InputUtilities.GetInputWithinTimeLimit()?.Trim();

            if (depositInput?.ToLower() == "back") // Check if the user wants to go back
            {
                initialDeposit = -1; // Set a flag for the calling method to go back
                return initialDeposit; // Exit the method
            }

            // Check if the input is valid and can be parsed to decimal
            if (decimal.TryParse(depositInput, out initialDeposit) && initialDeposit >= 0)
            {
                // Check if the input meets the minimum requirement
                if ((!requireMinimum || initialDeposit >= 1) && (!requireMaximum || initialDeposit <= 20000))
                {
                    isValidInput = true; // Exit loop if the input is valid
                }
                else
                {
                    if (requireMaximum)
                    {
                        Console.WriteLine("Invalid initial deposit amount. Please provide an amount between £1 and £20,000.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid initial deposit amount. Please provide at least £1.");
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid integer amount.");
            }
        } while (!isValidInput);

        return initialDeposit;
    }

    // GetUserInput Method
    private static string GetUserInput(string prompt)
    {
        string? userInput;
        do
        {
            Console.Write(prompt);
            userInput = InputUtilities.GetInputWithinTimeLimit()?.Trim().ToLower();

            // Verify if the input is "confirm"
            if (userInput != "confirm" && userInput != "back")
            {
                Console.WriteLine("Invalid input. Please type 'confirm' to proceed.");
            }
        } while (userInput != "confirm" && userInput != "back");

        return userInput;
    }

    // GetDateOfBirth Method
    public static DateTime GetDateOfBirth()
    {
        DateTime dob;
        string? dobInput;
        do
        {
            Console.Write("Please, provide your date of birth in this format -> DD-MM-YYYY: ");
            dobInput = InputUtilities.GetInputWithinTimeLimit()?.Trim();

            if (dobInput?.ToLower() == "back")
            {
                dob = DateTime.MinValue; // Set a flag for the calling method to go back
                return dob;
            }
            if (!DateTime.TryParse(dobInput, out dob))
            {
                Console.WriteLine("Invalid date of birth format. Please try again.");
            }
        } while (dob == DateTime.MinValue); // Ensures the loop runs until a valid date is entered
        return dob;
    }

    // Generate account number method
    private static string GenerateAccountNumber()
    {
        Random random = new Random();
        return random.Next(10000000, 99999999).ToString(); // Generating a random 8-digit account number
    }

    // Save account number method
    private static void SaveAccountNumber(string accountNumber)
    {
        string directory = $@"{AppDomain.CurrentDomain.BaseDirectory}\Accounts\";
        string filePath = $@"{directory}{accountNumber}.txt";
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        // Save the account number to a file
        File.WriteAllText(filePath, accountNumber);
        Console.WriteLine($"Account number {accountNumber} saved successfully.");
    }

    // Generate sort code method
    private static string GenerateSortCode()
    {
        Random random = new Random();
        return random.Next(100000, 999999).ToString(); // Generating a random 6-digit sort code
    }

    // Save sort code method with customer ID parameter
    private static void SaveSortCode(string sortCode, string customerId)
    {
        string directory = $@"{AppDomain.CurrentDomain.BaseDirectory}\Accounts\";
        string filePath = $@"{directory}{customerId}{sortCode}_sortCode.txt";
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        // Save the sort code to a file
        File.WriteAllText(filePath, sortCode);
        Console.WriteLine($"Sort code {sortCode} saved successfully for customer {customerId}.");
    }

    // Append account details to CSV
    private static void AppendToCsv(string accountType, string accountNumber, string sortCode)
    {
        try
        {
            // Open the CSV file in append mode and write the account details
            using (StreamWriter sw = File.AppendText(accountCsvFile))
            {
                sw.WriteLine($"{accountType},{accountNumber},{sortCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error appending to CSV: {ex.Message}");
        }
    }

    // Generate sort code method with customer ID parameter
    private static string GenerateSortCode(string customerId)
    {
        Random random = new Random();
        return $"{customerId.Substring(0, Math.Min(6, customerId.Length))}{random.Next(100000, 999999)}"; // Generating a sort code based on customer ID
    }
}