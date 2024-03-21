namespace AccountCreation.AccountCreation;
public class AccountCreation
{
    // DisplayMenu Mehtod
    public static void DisplayMenu()
    {
        bool condition = true;

        while (condition)
        {
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
            string? choice = Console.ReadLine();

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

        // Request date of birth
        DateTime dob = GetDateOfBirth();
        if (dob == DateTime.MinValue) // Check if the user wants to go back
        {
            return;
        }

        // Verify age (minimum age: 18)
        if (DateTime.Today.Subtract(dob).TotalDays / 365 < 18)
        {
            Console.WriteLine("You must be at least 18 years old to open a personal account. Account creation failed.");
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

    }

    // CreateISA Method
    private static void CreateISA()
    {
        // Check if the customer already has an ISA account
        if (CheckForISA())
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

        // Request date of birth
        DateTime dob = GetDateOfBirth();
        if (dob == DateTime.MinValue) // Check if the user wants to go back
        {
            return;
        }

        // Verify age (Minimum age: 16)
        if (DateTime.Today.Subtract(dob).TotalDays / 365 < 16)
        {
            Console.WriteLine("You must be at least 16 years old to open an ISA account. Account creation failed.");
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
    }

    // CreateBusinessAccount Method
    private static void CreateBusinessAccount()
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
            businessType = Console.ReadLine()?.Trim();

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
            depositInput = Console.ReadLine()?.Trim();

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

    // GetUserInput Mehtod
    private static string GetUserInput(string prompt)
    {
        string? userInput;
        do
        {
            Console.Write(prompt);
            userInput = Console.ReadLine()?.Trim().ToLower();

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
            dobInput = Console.ReadLine()?.Trim();

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
}