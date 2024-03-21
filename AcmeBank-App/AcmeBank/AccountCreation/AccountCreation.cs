using AcmeBank;

namespace AccountCreation.AccountCreation;
public class AccountCreation
{
    // DisplayMenu Mehtod
    public static void DisplayMenu()
    {
        bool condition = true;

        while (condition)
        {
            /*Console.Clear();*/
            Console.Write("""
                ============================
                    **ACCOUNT CREATION** 
                ============================
                1. Personal Account.
                2. Individual Savings Account (ISA).
                3. Business Account.
                ============================
                Please, Select Account Type: 
                """);
            string? choice = InputUtilities.GetInputWithinTimeLimit();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("\nCreating Personal Account...");
                    // Call personal account creation method
                    CreatePersonalAccount();
                    break;
                case "2":
                    Console.WriteLine("\nCreating Individual Savings Account (ISA)...");
                    // Call ISA account creation method
                    CreateISA();
                    break;
                case "3":
                    Console.WriteLine("\nCreating Business Account...");
                    // Call business account creation method
                    CreateBusinessAccount();
                    break;
                default:
                    Console.WriteLine("\nInvalid choice. Please try again.");
                    break;
            }
        }
    }

    // CreatePersonalAccount Mehtod
    private static void CreatePersonalAccount()
    {
        // Request photo ID until the user confirms identity
        string photoID = GetUserInput("Please, provide photo ID (type 'confirm' to proceed): ");

        // Request address ID until the user confirms identity
        string addressID = GetUserInput("Please, provide address ID (type 'confirm' to proceed): ");

        // Request date of birth
        DateTime dob = GetDateOfBirth();

        // Verify age (minimum age: 18)
        if (DateTime.Today.Subtract(dob).TotalDays / 365 < 18)
        {
            Console.WriteLine("You must be at least 18 years old to open a personal account. Account creation failed.");
            return;
        }

        // Request initial deposit
        decimal initialDeposit = RequestInitialDeposit(true);

        // Personal account created auccessfully
        Console.WriteLine("Personal account created successfully!");
    }

    // CreateISA Mehtod
    private static void CreateISA()
    {
        // Check if the customer already has an ISA account
        if (CheckForISA())
        {
            Console.WriteLine("Customer already has an ISA account. Account creation canceled.");
            return;
        }

        // Request photo and address ID
        string photoID = GetUserInput("Please, provide photo ID (type 'confirm' to proceed): ");
        string addressID = GetUserInput("Please, provide address ID (type 'confirm' to proceed): ");

        DateTime dob = GetDateOfBirth();

        // Verify age (Minimum age: 16)
        if (DateTime.Today.Subtract(dob).TotalDays / 365 < 16)
        {
            Console.WriteLine("You must be at least 16 years old to open an ISA account. Account creation failed.");
            return;
        }

        // ISA account created auccessfully
        Console.WriteLine("Individual Savings Account (ISA) created successfully!");
    }

    // CreateBusinessAccount Mehtod
    private static void CreateBusinessAccount()
    {
        // List of valid business types
        List<string> validBusinessTypes = new List<string>
       {
           "Enterprise",
           "PLC",
           "Charity",
           "Public Sector"
       };

        // Prompt user to select a business type
        Console.WriteLine("Please select the business type from the following options: ");
        foreach (var type in validBusinessTypes)
        {
            Console.WriteLine($"- {type}");
        }

        string? businessType;
        do
        {
            // Request user input for business type
            Console.Write("Enter the business type: ");
            businessType = InputUtilities.GetInputWithinTimeLimit()?.Trim();

            // Check if the entered business type is valid
            if (!validBusinessTypes.Contains(businessType, StringComparer.OrdinalIgnoreCase))
            {
                Console.WriteLine("Invalid business type. Please choose from the provided options.");
            }
        } while (!validBusinessTypes.Contains(businessType, StringComparer.OrdinalIgnoreCase));

        // Request proof of business existence and details
        string confirmBusinessDetails = GetUserInput("Please, type 'confirm' to prove your business existence and its details: ");
        if (confirmBusinessDetails != "confirm")
        {
            Console.WriteLine("You have not confirmed your business. Please try again!");
            return;
        }

        // Store business details (Not stored anywhere - YET)
        Console.WriteLine("Your business details have been stored successfully!");

        // Request initial deposit with no minimum requirement
        decimal initialDeposit = RequestInitialDeposit(false);

        // No minimum initial deposit for business account

        // If all the checks pass, then the account is created
        Console.WriteLine("Business account created successfully!");
    }

    // CheckForISA Mehtod
    private static bool CheckForISA()
    {
        // Dummy implementation to check if customer already has an ISA account
        // This could be replaced with actual logic to check in the database or CSV
        // For demonstration purpose, always return false (indicating customer doesn't have an ISA account)
        return false;
    }

    // RequestInitialDeposit Mehtod
    private static decimal RequestInitialDeposit(bool requireMinimum)
    {
        // Prompt message based on requirement
        string promptMessage = requireMinimum ? "Please, provide the initial deposit amount (at least £1): " : "Please, provide the initial deposit amount (enter £0 or greater): ";

        // Request initial deposit amount
        decimal initialDeposit;
        string? depositInput;
        bool isValidInput = false;

        do
        {
            Console.Write(promptMessage);
            depositInput = InputUtilities.GetInputWithinTimeLimit();

            // Check if the input is valid and can be parsed to decimal
            if (decimal.TryParse(depositInput, out initialDeposit) && initialDeposit >= 0)
            {
                // Check if the input meets the minimum requirement
                if (!requireMinimum || initialDeposit >= 1)
                {
                    isValidInput = true; // Exit loop if the input is valid
                }
                else
                {
                    Console.WriteLine("Invalid initial deposit amount. Please provide at least £1.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid integer amount.");
            }
        } while (!isValidInput);

        return initialDeposit;
    }

    // GetUserInput Mehtod
    private static string GetUserInput(string prompt)
    {
        string? userInput;
        do
        {
            Console.Write(prompt);
            userInput = InputUtilities.GetInputWithinTimeLimit()?.Trim().ToLower();

            // Verify if the input is "confirm"
            if (userInput != "confirm")
            {
                Console.WriteLine("Invalid input. Please type 'confirm' to proceed.");
            }
        } while (userInput != "confirm");

        return userInput;
    }

    // GetDateOfBirth Method
    public static DateTime GetDateOfBirth()
    {
        DateTime dob;
        do
        {
            Console.WriteLine("Please, provide your date of birth in this format -> DD-MM-YYYY: ");
            string? dobInput = InputUtilities.GetInputWithinTimeLimit();
            if (!DateTime.TryParse(dobInput, out dob))
            {
                Console.WriteLine("Invalid date of birth format. Please try again.");
            }
        } while (dob == DateTime.MinValue); // Ensures the loop runs until a valid date is entered
        return dob;
    }
}