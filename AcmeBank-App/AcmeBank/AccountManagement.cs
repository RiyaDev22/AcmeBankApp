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

    //we want a method that will handle the payment process, as it's used in multiple places
    private static void HandlePayment()
    {
        //TODO: we need verification here, so we'll need some loops and checks
        //we display the user's accounts, and ask for the destination account and amount to send
        string destination = HandleInput("Please enter the account number you would like to pay to: ");
        string paymentAmount = HandleInput("Please enter the amount you would like to pay: ");
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
                HandlePayment();
                break;
            case "8":
                //simply inform the customer that a debit card will be sent to their address
                Console.WriteLine("A debit card will be sent to [ADDRESS]");
                break;
            case "9":
                //we need a submenu for standing orders, so we handle those here
                //TODO: we should probably break this out into a method at least, readability is getting rough
                Console.WriteLine("""
                    Please select an option:
                        1. View Standing Orders
                        2. Manage Standing Orders
                        X. Cancel
                    """);
                string standingOrderInput = HandleInput("Please enter your selection: ");
                //TODO: actually handle the options, and make su
                break;
            case "10":
                //we use a similar process for direct debits
                //TODO: also could be broken out into a method
                Console.WriteLine("""
                    Please select an option:
                        1. View Standing Orders
                        2. Manage Standing Orders
                        X. Cancel
                    """);
                string directDebitInput = HandleInput("Please enter your selection: ");
                //TODO: actually handle the options
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
                HandlePayment();
                break;
            case "8":
                //we make a submenu for the card options, as we have two options here
                string cardInput = HandleInput("""
                    Please select the type of card you would like to request:
                        1. Credit Card
                        2. Debit Card
                        X. Cancel
                    """);
                //TODO: actually handle the options
                break;
            case "9":
                //simply inform the customer that a cheque book will be sent to their address
                Console.WriteLine("A cheque book will be sent to [ADDRESS]");
                break;
            case "10":
                //we need a submenu for overdrafts, since we have a few options for them
                string overdraftInput = HandleInput("""
                    Please select an option:
                        1. View Overdraft Limit
                        2. Manage Overdraft
                        X. Cancel
                    """);
                //TODO: actually handle the options
                break;
            case "11":
                //we inform the user that they are eligible for business loans. this is not something we handle in this app, but we can inform the user
                Console.WriteLine("This acount is eligible for business loans. Please direct them to the Loans department.");
                break;
            case "12":
                //we inform the user that they are eligible for international transfers. we also don't handle those in this app
                Console.WriteLine("This account is eligible for international transfers. Please direct them to the International Transfers department.");
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
                5. Freeze/Unfreeze Account
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
                        //ask for a number to deposit
                        string depositAmount = HandleInput("Please enter the amount you would like to deposit: ");
                        //TODO: add the money to the account
                        break;
                    case "2":
                        //ask for a number to withdraw
                        string withdrawAmount = HandleInput("Please enter the amount you would like to withdraw: ");
                        //TODO: subtract the money from the account
                        break;
                    case "3":
                        //show the user their accounts, and ask for the destination account and amount to transfer
                        Console.WriteLine("[LIST OF ACCOUNTS UNDER CURRENT CUSTOMER]");
                        string destination = HandleInput("Please enter the account number you would like to transfer to: ");
                        string transferAmount = HandleInput("Please enter the amount you would like to transfer: ");
                        //TODO: transfer the money from this account to the destination account
                        break;
                    case "4":
                        //generate a statement for a selected period
                        Console.WriteLine("Generating Statement...");
                        string startDate = HandleInput("Please enter the start date for the statement: ");
                        string endDate = HandleInput("Please enter the end date for the statement: ");
                        //TODO: generate a statement for the account for the selected period
                        break;
                    case "5":
                        //get the account's frozen status
                        bool accountFrozen = false;
                        //if the account is not frozen, we freeze it
                        if (!accountFrozen)
                        {
                            Console.WriteLine("This account has been frozen.");
                        }
                        //if the account is frozen, we unfreeze it
                        //TODO: we should probably ask for some form of verification here
                        else
                        {
                            Console.WriteLine("This account has been unfrozen.");
                        }
                        break;
                    case "6":
                        //close the account
                        //TODO: we definitely need verification here, and to handle the account's closure
                        Console.WriteLine("Closing Account...");
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