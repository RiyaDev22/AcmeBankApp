using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using AcmeBank.BankAccounts.AccountInterfaces;
using AcmeBank.BankAccounts.Transactions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AcmeBank.BankAccounts.RegularPayments
{
    internal class RegularPaymentUtilities
    {
        private static Account _currentAccount { get; set; }
        private static Customer _currentCustomer { get; set; }
        private static string directory = $@"{AppDomain.CurrentDomain.BaseDirectory}\Accounts\";
        internal static void RegularPaymentOptions(Account account, Customer customer)
        {
            _currentAccount = account;
            _currentCustomer = customer;
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

                x. Exit
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
                    ManageDDs(ref invalidPrompt);
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

                x. Exit
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
                    ManageSOs(ref invalidPrompt);
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

            bool exit = false;
            // Loop until a valid payee account is selected and a valid payment amount is entered
            do
            {
                // Get payee details (sort code and account number)
                TransactionUtilities.GetPayeeDetails(out string sortCode, out string accountNumber, invalidAccountNumbers, ref exit);

                if(exit) { return; }

                payeeAccount = AccountUtilities.LoadAccountDetails($"{accountNumber}",_currentCustomer); // Load payee account details based on the provided account number

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
                Console.WriteLine($"""
                --- Standing Orders ---
                From: {_currentAccount.AccountNumber}
                To: {payeeAccount.AccountNumber}
                -----------------------
                ## Please provide the amount for the standing order.
                <- Enter 'x' to exit.
                """);


                // Display any previous error messages
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(amountInvalidPrompt.ToString());
                amountInvalidPrompt.Clear();
                Console.ResetColor();

                Console.Write("Amount: ");
                amountInput = Console.ReadLine();

                if (amountInput.ToLower() == "x")
                    return;
                else if (!decimal.TryParse(amountInput, out amount))
                    amountInvalidPrompt.Append("!!! Must be a number !!!");
                else if (amount <= 0)
                    amountInvalidPrompt.Append("!!! Must be greater than 0 !!!");
                else
                    amountValid = true;
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
                Console.WriteLine($"""
                --- Standing Orders ---
                From: {_currentAccount.AccountNumber}
                To: {payeeAccount.AccountNumber}
                Amount: {amount:C2}
                -----------------------
                ## When would you like monthly payments to start.
                ## Enter a date in the format (DD-MM-YYYY).
                <- Enter x to exit.
                """);

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(startDateInvalidPrompt);
                startDateInvalidPrompt.Clear();
                Console.ResetColor();

                Console.Write("Date: ");
                startDateInput = Console.ReadLine();

                bool checkDate = DateTime.TryParse(startDateInput, out startDate);
                if (startDateInput.ToLower() == "x")
                    return;
                if (!(dateRegex.IsMatch(startDateInput) && checkDate))
                    startDateInvalidPrompt.Append("!!! Invalid date - must follow the format (DD-MM-YYYY) !!!");
                else if (startDate <= DateTime.Today)
                    startDateInvalidPrompt.Append("!!! Invalid date - cannot be a date in the past !!!");
                else
                    dateValid = true;
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
                ## Can you confirm this is correct.
                ## Enter 'y' for yes or any other key for no.
                
                Your choice: 
                """);

            string? confirmInput = Console.ReadLine();
            if (confirmInput.ToLower() == "y")
            {
                // Save to a file
                RegularPayment standingOrder = new RegularPayment(payeeAccount.AccountNumber, amount, startDate);
                SaveSO(_currentAccount.AccountNumber, standingOrder);
                // Display saved prompt
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("STANDING ORDER SAVED");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("STANDING ORDER CANCELLED");
                Console.ResetColor();
            }
            Thread.Sleep(1000); // Pause for 1 second
        }

        private static void SaveSO(string thisAccountNumber, RegularPayment standingOrder)
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
            sb.AppendLine($"{standingOrder.PayeeAccountNumber},{standingOrder.Amount},{standingOrder.Date},Monthly,");

            // Append the transaction details to the CSV file
            File.AppendAllText(path, sb.ToString());
        }


        private static List<RegularPayment> LoadSOs(string accountNumberToLoad)
        {
            // Construct the file directory path
            string fileDirectory = $@"{directory}\{accountNumberToLoad}";
            string path = $@"{fileDirectory}\StandingOrders.csv";

            List<RegularPayment> regularPayments = new List<RegularPayment>();

            try
            {
                if (File.Exists(path))
                {
                    // Read the file
                    string[] payments = File.ReadAllLines(path);
                    foreach (string regularPayment in payments)
                    {
                        string[] regularPaymentSplit = regularPayment.Split(',');
                        string payeeAccountNumber = regularPaymentSplit[0];
                        decimal amount = decimal.Parse(regularPaymentSplit[1]);
                        DateTime date = DateTime.Parse(regularPaymentSplit[2]);
                        regularPayments.Add(new RegularPayment(payeeAccountNumber, amount, date));
                    }
                    return regularPayments;
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

            return regularPayments;
        }

        private static void ManageSOs(ref StringBuilder invalidMenuPrompt)
        {
            StringBuilder standingOrderPrompt = new StringBuilder();
            StringBuilder invalidOptionPrompt = new StringBuilder();

            bool exit = false;
            while (!exit)
            {
                Console.Clear();

                List<RegularPayment> standingOrders = LoadSOs(_currentAccount.AccountNumber);

                if (standingOrders.Count <= 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    invalidMenuPrompt.Append("!!! Customer has no standing orders !!!");
                    Console.ResetColor();
                    return;
                }


                int count = 1;
                standingOrderPrompt.Clear();
                foreach (RegularPayment regularPayment in standingOrders)
                {
                    standingOrderPrompt.AppendLine($"{count,3}: {regularPayment.PayeeAccountNumber,15} {regularPayment.Amount,15:C2} {regularPayment.Date.Date,15:D}");
                    count++;
                }

                
                Console.Write("""
                    --- Manage Standing Orders  ---
                    ## Enter the id of the any order you would like to cancel e.g '1'.
                    <- Enter 'x' to exit.

                    ID: 
                    """);

                // Save the current cursor position
                int currentLeft = Console.CursorLeft;
                int currentTop = Console.CursorTop;

                if (invalidOptionPrompt.ToString() != "")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n{invalidOptionPrompt}");
                    invalidOptionPrompt.Clear();
                    Console.ResetColor();
                }

                Console.WriteLine($"""

                        
                    ----------------- Standing Orders ------------------
                      id   Payee Account          Amount            Date
                    {standingOrderPrompt}
                    ----------------------------------------------------
                    """);

                // Move the cursor position to the line where user input is expected
                Console.SetCursorPosition(currentLeft, currentTop);

                // Then provide the option to send statement or exit
                string? input = Console.ReadLine();
                int id;
                bool validID = int.TryParse(input, out id);
                if (input.ToLower() == "x")
                {
                    exit = true;
                } 
                else if (validID && id > 0 && id <= standingOrders.Count)
                {
                    standingOrders.RemoveAt(id-1);
                    // Save remaining
                    UpdateSOs(standingOrders);
                    if(standingOrders.Count <= 0) { exit = true; }
                } 
                else
                {
                    invalidOptionPrompt.AppendLine("!!! Invalid ID !!!");
                }
            }

        }

        private static void UpdateSOs(List<RegularPayment> standingOrders)
        {
            string fileDirectory = $@"{directory}\{_currentAccount.AccountNumber}"; // Construct the file directory path

            // Check if the directory exists, if not, create it
            if (!Directory.Exists(fileDirectory))
            {
                Directory.CreateDirectory(fileDirectory);
                using (File.Create($@"{fileDirectory}\StandingOrders.csv")) ;
            }

            // Construct the path for AccountDetails.csv
            string path = @$"{fileDirectory}\StandingOrders.csv";

            // Create a StringBuilder to construct the CSV content
            StringBuilder sb = new StringBuilder();
            foreach(RegularPayment payment in standingOrders)
            {
                sb.AppendLine($"{payment.PayeeAccountNumber},{payment.Amount},{payment.Date},Monthly,");
            }
            // Write the CSV content to the file
            File.WriteAllText(path, sb.ToString());
        }

        private static void ManageDDs(ref StringBuilder invalidMenuPrompt)
        {
            StringBuilder directDebitPrompt = new StringBuilder();
            StringBuilder invalidOptionPrompt = new StringBuilder();

            bool exit = false;
            while (!exit)
            {
                Console.Clear();

                List<RegularPayment> directDebits = LoadDDs(_currentAccount.AccountNumber);

                if (directDebits.Count <= 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    invalidMenuPrompt.Append("!!! Customer has no direct debits !!!");
                    Console.ResetColor();
                    return;
                }


                int count = 1;
                directDebitPrompt.Clear();
                foreach (RegularPayment regularPayment in directDebits)
                {
                    directDebitPrompt.AppendLine($"{count,3}: {regularPayment.PayeeAccountNumber,15} {regularPayment.Amount,15:C2} {regularPayment.Date.Date,15:D}");
                    count++;
                }


                Console.Write("""
                    --- Manage Direct Debits  ---
                    Enter the id of the any order you would like to cancel e.g '1'.
                    Enter 'x' to exit.

                    ID: 
                    """);

                // Save the current cursor position
                int currentLeft = Console.CursorLeft;
                int currentTop = Console.CursorTop;

                if (invalidOptionPrompt.ToString() != "")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n{invalidOptionPrompt}");
                    invalidOptionPrompt.Clear();
                    Console.ResetColor();
                }

                Console.WriteLine($"""

                        
                    ------------------ Direct Debits -------------------
                      id Debtor Account          Amount            Date
                    {directDebitPrompt}
                    ----------------------------------------------------
                    """);

                // Move the cursor position to the line where user input is expected
                Console.SetCursorPosition(currentLeft, currentTop);

                // Then provide the option to send statement or exit
                string? input = Console.ReadLine();
                int id;
                bool validID = int.TryParse(input, out id);
                if (input.ToLower() == "x")
                {
                    exit = true;
                } else if (validID && id > 0 && id <= directDebits.Count)
                {
                    directDebits.RemoveAt(id - 1);
                    // Save remaining
                    UpdateDDs(directDebits);
                    if (directDebits.Count <= 0) { exit = true; }
                } else
                {
                    invalidOptionPrompt.AppendLine("!!! Invalid ID !!!");
                }
            }
        }

        private static void UpdateDDs(List<RegularPayment> directDebits)
        {
            string fileDirectory = $@"{directory}\{_currentAccount.AccountNumber}"; // Construct the file directory path

            // Check if the directory exists, if not, create it
            if (!Directory.Exists(fileDirectory))
            {
                Directory.CreateDirectory(fileDirectory);
                using (File.Create($@"{fileDirectory}\DirectDebits.csv")) ;
            }

            // Construct the path for AccountDetails.csv
            string path = @$"{fileDirectory}\DirectDebits.csv";

            // Create a StringBuilder to construct the CSV content
            StringBuilder sb = new StringBuilder();
            foreach (RegularPayment payment in directDebits)
            {
                sb.AppendLine($"{payment.PayeeAccountNumber},{payment.Amount},{payment.Date},Monthly,");
            }
            // Write the CSV content to the file
            File.WriteAllText(path, sb.ToString());
        }

        private static List<RegularPayment> LoadDDs(string accountNumberToLoad)
        {
            // Construct the file directory path
            string fileDirectory = $@"{directory}\{accountNumberToLoad}";
            string path = $@"{fileDirectory}\DirectDebits.csv";

            List<RegularPayment> regularPayments = new List<RegularPayment>();

            try
            {
                if (File.Exists(path))
                {
                    // Read the file
                    string[] payments = File.ReadAllLines(path);
                    foreach (string regularPayment in payments)
                    {
                        string[] regularPaymentSplit = regularPayment.Split(',');
                        string payeeAccountNumber = regularPaymentSplit[0];
                        decimal amount = decimal.Parse(regularPaymentSplit[1]);
                        DateTime date = DateTime.Parse(regularPaymentSplit[2]);
                        regularPayments.Add(new RegularPayment(payeeAccountNumber, amount, date));
                    }
                    return regularPayments;
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

            return regularPayments;
        }
    }
}
