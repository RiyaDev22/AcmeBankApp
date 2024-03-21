using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace AcmeBank.BankAccounts.Transactions
{
    internal class RegularPayments
    {
        private static Account _currentAccount { get; set; }
        private static string directory = $@"{AppDomain.CurrentDomain.BaseDirectory}\Accounts\";
        internal static void RegularPaymentOptions(Account account)
        {
            _currentAccount = account;
            //Display a menu asking either standing orders or direct debits.
            StringBuilder invalidPrompt = new StringBuilder();

            bool exit = false; // Initialize a flag to control the loop
            while (!exit) // Loop until the user chooses to exit
            {
                // Display account details and options
                Console.Clear();
                Console.WriteLine("""
                --- Regular Payments ---
                1. Standing Orders
                2. Direct Debits
                X. Exit
                ------------------------
                """);

                // Display any error messages
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(invalidPrompt.ToString());
                Console.ResetColor();
                invalidPrompt.Clear();

                // Ask the user to enter an option
                Console.Write("Enter an option: ");
                string optionInput = Console.ReadLine();

                exit = HandleRPOption(optionInput, ref invalidPrompt);
            }
        }

        private static bool HandleRPOption(string? optionInput, ref StringBuilder invalidPrompt)
        {
            switch (optionInput.ToLower()) // Process the user's choice
            {
                case "1":
                    StandingOrderOptions();
                    break;
                case "2":
                    Console.WriteLine("Direct Debits");
                    break;
                case "x":
                    // Exit the loop if the user chooses to exit
                    return true;
                default:
                    // Display an error message if the user enters an invalid option
                    Console.Clear();
                    invalidPrompt.Append("-- !!! Invalid option !!! --");
                    break;
            }
            return false; //does not exit the loop
        }

        private static void StandingOrderOptions()
        {
            //Display a menu asking either standing orders or direct debits.
            StringBuilder invalidPrompt = new StringBuilder();

            bool exit = false; // Initialize a flag to control the loop
            while (!exit) // Loop until the user chooses to exit
            {
                // Display account details and options
                Console.Clear();
                Console.WriteLine("""
                --- Standing Orders ---
                1. Setup Standing Order
                2. Manage Standing Orders
                X. Exit
                -----------------------
                """);

                // Display any error messages
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(invalidPrompt.ToString());
                Console.ResetColor();
                invalidPrompt.Clear();

                // Ask the user to enter an option
                Console.Write("Enter an option: ");
                string optionInput = Console.ReadLine();

                exit = HandleSOOption(optionInput, ref invalidPrompt);
            }
        }

        private static bool HandleSOOption(string? optionInput, ref StringBuilder invalidPrompt)
        {
            switch (optionInput.ToLower()) // Process the user's choice
            {
                case "1":
                    SetupSO();
                    break;
                case "2":
                    ManageSOs();
                    break;
                case "x":
                    // Exit the loop if the user chooses to exit
                    return true;
                default:
                    // Display an error message if the user enters an invalid option
                    Console.Clear();
                    invalidPrompt.Append("-- !!! Invalid option !!! --");
                    break;
            }
            return false; //does not exit the loop
        }

        private static void SetupSO()
        {
            // Initialize variables for payee account details, payment amount, and error messages
            Account payeeAccount = null;
            string? input;
            
            StringBuilder invalidPrompt = new StringBuilder();
            List<string> invalidAccountNumbers = new List<string>() { _currentAccount.AccountNumber };

            // Loop until a valid payee account is selected and a valid payment amount is entered
            do
            {
                // Get payee details (sort code and account number)
                TransactionUtilities.GetPayeeDetails(out string sortCode, out string accountNumber, invalidAccountNumbers);
                payeeAccount = AccountUtilities.LoadAccountDetails($"{accountNumber}"); // Load payee account details based on the provided account number

                // Checks if the payee is a savings account if so we provide an error prompt and return preventing the payment
                if (payeeAccount.Type == AccountType.ISA)
                {
                    payeeAccount = null;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("!!! Can not make a payment to a savings account !!!");
                    Console.ResetColor();

                    Thread.Sleep(1500); // Pause execution briefly to display the error message
                    Console.Clear();
                }

            } while (payeeAccount == null);


            bool amountValid = false;
            decimal amount = 0;
            string? amountInput;
            StringBuilder amountInvalidPrompt = new StringBuilder();
            while (!amountValid)
            {
                // Display account details and payment header including account from and to
                Console.Clear();
                Console.Write($"""
                --- Standing Orders ---
                From: {_currentAccount.AccountNumber}
                To: {payeeAccount.AccountNumber}
                -----------------------
                Please provide the amount for the standing order

                Amount: 
                """);

                // Save the current cursor position
                int currentLeft = Console.CursorLeft;
                int currentTop = Console.CursorTop;

                // Display any previous error messages
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(amountInvalidPrompt.ToString());
                amountInvalidPrompt.Clear();
                Console.ResetColor();

                Console.SetCursorPosition(currentLeft, currentTop);
                amountInput = Console.ReadLine();

                if(decimal.TryParse(amountInput, out amount) && amount > 0)
                    amountValid = true;
                else
                    amountInvalidPrompt.Append("""


                        !!! Invalid amount !!!
                        - must a number
                        - cannot be less than 0
                        """);

            }


            //Ask how frequently we need to make the payment and its start date.
            DateTime startDate = new DateTime();
            Regex dateRegex = new Regex(@"^(0[1-9]|[1-2][0-9]|3[0-1])-(0[1-9]|1[0-2])-\d{4}$");

            string? startDateInput;
            StringBuilder startDateInvalidPrompt = new StringBuilder();
            bool dateValid = false;
            while (!dateValid)
            {
                Console.Clear();
                Console.Write($"""
                --- Standing Orders ---
                From: {_currentAccount.AccountNumber}
                To: {payeeAccount.AccountNumber}
                Amount: {amount:C2}
                -----------------------
                When would you like monthly payments to start 
                Enter a date in the format (DD-MM-YYYY)
                
                Date: 
                """);

                // Save the current cursor position
                int currentLeft = Console.CursorLeft;
                int currentTop = Console.CursorTop;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(startDateInvalidPrompt);
                startDateInvalidPrompt.Clear();
                Console.ResetColor();

                Console.SetCursorPosition(currentLeft, currentTop);
                startDateInput = Console.ReadLine();
                if (dateRegex.IsMatch(startDateInput) && DateTime.TryParse(startDateInput, out startDate) && startDate >= DateTime.Today)
                    dateValid = true;
                else
                    startDateInvalidPrompt.Append("""


                        !!! Invalid date !!!
                        - must follow the format (DD-MM-YYYY)
                        - cannot be a date in the past
                        """);
            }


            //ask to confirm and then save to file
            Console.Clear();
            Console.Write($"""
                --- Standing Orders ---
                From: {_currentAccount.AccountNumber}
                To: {payeeAccount.AccountNumber}
                Amount: {amount:C2}
                Date: {startDate:D}
                -----------------------
                Can you confirm this is correct
                Enter 'y' for yes or any other key for no.
                
                Your choice: 
                """);

            string? confirmInput = Console.ReadLine();
            if (confirmInput.ToLower() == "y")
            {
                // Save to a file
                SaveSO(_currentAccount.AccountNumber,payeeAccount.AccountNumber,amount,startDate);
                // Display saved prompt
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("STANDING ORDER SAVED");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("STANDING ORDER CANCELLED");
                Console.ResetColor();
            }
            Thread.Sleep(1000); // Pause for 1 second
        }

        internal static void SaveSO(string thisAccountNumber, string payeeAccountNumber, decimal amount, DateTime date)
        {
            // Construct the file directory path
            string fileDirectory = $@"{directory}\{thisAccountNumber}";

            // Create the directory if it doesn't exist
            if (!Directory.Exists(fileDirectory))
            {
                Directory.CreateDirectory(fileDirectory);
                using (File.Create($@"{fileDirectory}\StandingOrders.csv")) ;
            }

            // Construct the file path
            string path = @$"{fileDirectory}\StandingOrders.csv";

            // Construct the string representation of the transaction
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{payeeAccountNumber},{amount},{date},Monthly");

            // Append the transaction details to the CSV file
            File.AppendAllText(path, sb.ToString());
        }

        private static void ManageSOs()
        {

        }
    }
}
