namespace AcmeBank;

class AccountManagement
{
    //this function is used to handle input from the user
    //it takes a string prompt, which is the message to display to the user
    private static string HandleInput(string prompt)
    {
        //we initialise the input as an empty string, and a bool to check if we need to try again
        string? input = "";
        bool tryAgain = true;
        do
        {
            //we display the prompt, and get the input from the user
            Console.Write(prompt);
            input = Console.ReadLine();
            //we set tryAgain to true if the input is empty, so we can try again
            tryAgain = string.IsNullOrEmpty(input);
            //if the input is empty, we display an error message
            if (tryAgain)
            {
                Console.WriteLine("Input cannot be empty, please try again.");
            }
            //we keep doing this until the input is not empty
        } while (tryAgain);
        //and then return the input
        return input;
    }

    //these functions are used to handle the input for the specific account types
    //we already handle cases 1-6 in the main menu, so we only need to handle the extra options here
    private static void PersonalAccountInput(string input)
    {
        switch (input)
        {
            //we want to do nothing here if the input is 1-6, as it would have been handled already
            //TODO: actually handle the appropriate options
            case "1":
            case "2":
            case "3":
            case "4":
            case "5":
            case "6":
                break;
            case "7":
                Console.WriteLine("HANDLE PAYMENT");
                break;
            case "8":
                Console.WriteLine("HANDLE DEBIT CARD");
                break;
            case "9":
                Console.WriteLine("HANDLE STANDING ORDERS");
                break;
            case "10":
                Console.WriteLine("HANDLE DIRECT DEBITS");
                break;
            case "11":
                Console.WriteLine("HANDLE OVERDRAFT");
                break;
        }
    }

    //we repeat this process for the options available to business accounts
    private static void BusinessAccountInput(string input)
    {
        switch (input)
        {
            //we want to do nothing here if the input is 1-6, as it would have been handled already
            //TODO: actually handle the appropriate options
            case "1":
            case "2":
            case "3":
            case "4":
            case "5":
            case "6":
                break;
            case "7":
                Console.WriteLine("HANDLE PAYMENT");
                break;
            case "8":
                Console.WriteLine("HANDLE CREDIT/DEBIT CARD");
                break;
            case "9":
                Console.WriteLine("HANDLE CHEQUE BOOK");
                break;
            case "10":
                Console.WriteLine("HANDLE OVERDRAFT");
                break;
            case "11":
                Console.WriteLine("HANDLE BUSINESS LOANS");
                break;
            case "12":
                Console.WriteLine("HANDLE INTERNATIONAL TRANSFERS");
                break;
        }
    }

    //TODO: change accType to the actual account type, and pass in the account's details
    public static void AccountMenu(string accType)
    {
        //TODO: fill these with correct values from the data
        //display account details
        Console.WriteLine($"Account Number: [PLACEHOLDER]");
        Console.WriteLine($"Account Holder: [PLACEHOLDER]");
        Console.WriteLine();
        Console.WriteLine($"Account Balance: [PLACEHOLDER]");

        //we have a bool to make sure the account type is valid. This shouldn't end up being used, but it's a good safety net
        bool validType = false;

        //we initialise the menText with the basic options
        string menuText = """
            Please select an option using the number provided, or enter 'help' for help:
                1. Deposit
                2. Withdraw
                3. Transfer
                4. Generate Statement
                5. Freeze Account
                6. Close Account
            """;

        //based on the account type, we add the relevant options to the menu
        switch (accType)
        {
            case "personal":
                //we set the validType to true, as we have a valid account type
                validType = true;
                //and display the account type
                Console.WriteLine("Account Type: Personal Account");
                //now we can add the relevant options to the menu
                menuText += """

                        7. Make Payment 
                        8. Request Debit Card
                        9. Manage Standing Orders
                        10. Manage Direct Debits
                        11. Manage Overdraft
                    """;
                break;

            case "business":
                //we run a similar process for business accounts
                validType = true;
                Console.WriteLine("Account Type: Business Account");
                menuText += """

                        7. Make Payment
                        8. Request Credit/Debit Card
                        9. Request Cheque Book
                        10. Manage Overdraft
                        11. Manage Business Loans
                        12. Manage International Transfers
                    """;
                break;

            case "isa":
                //ISAs have fewer options, so we just use the default ones common to all accounts
                validType = true;
                Console.WriteLine("Account Type: ISA Account ");
                break;

            //if the account type is not recognised, we display an error message
            //again, this should be impossible, but better safe than crashing
            default:
                Console.WriteLine("A data error has occured with this account. Please contact IT immediately.");
                break;
        }

        menuText += "\n\tX. Exit";

        //and display the menu, but only if the account type is valid
        if (validType)
        {
            //we have a bool to keep the menu running
            bool running = true;
            //and we keep running the menu until the user decides to exit
            do
            {
                //we output the menu's text
                Console.WriteLine(menuText);
                //and get the user's input
                string input = HandleInput("Please enter your selection: ");

                //we then handle the input based on the account type
                //we start with the common options, handling each appropriately

                //we need a bool to keep track of whether the input was handled in this switch
                bool handled = true;

                //TODO: actually handle these options
                switch (input)
                {
                    case "1":
                        Console.WriteLine("HANDLE DEPOSIT");
                        break;
                    case "2":
                        Console.WriteLine("HANDLE WITHDRAW");
                        break;
                    case "3":
                        Console.WriteLine("HANDLE TRANSFER");
                        break;
                    case "4":
                        Console.WriteLine("HANDLE STATEMENT");
                        break;
                    case "5":
                        Console.WriteLine("HANDLE FREEZE");
                        break;
                    case "6":
                        Console.WriteLine("HANDLE CLOSE");
                        break;
                    //if the user wants to exit, we set running to false
                    case "x":
                        running = false;
                        break;
                    //if the user wants help, we display the help message
                    case "help":
                        Console.WriteLine("HELP WILL BE SHOWN HERE");
                        break;
                    default:
                        handled = false;
                        break;
                }
                //if the input wasn't handled in the common options, we handle it based on the account type
                if (!handled)
                {
                    //based on the account type, we hand the input to a function to handle the options specific to that account type
                    switch (accType)
                    {
                        case "personal":
                            PersonalAccountInput(input);
                            break;
                        case "business":
                            BusinessAccountInput(input);
                            break;
                            //we don't need to do anything for ISAs, as they have no extra options
                    }
                }
                //we repeat this process until the user decides to exit
            } while (running);
        }
    }
}