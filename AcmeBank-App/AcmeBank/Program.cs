using AcmeBank.BankAccounts;

namespace AcmeBank
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Create a new Teller object which displays the login screen once the application starts
            Teller oTeller = new Teller();
            //Initialise a string list which will contain customer's details
            List<string> slCustomers = populateStringList();
            //Initialise a customer list which will contain customer's details from the string list
            List<Customer> clCustomers = populateCustomerList(slCustomers);

            //Declare object which will display the customer validation screen
            CustomerValidation oCustomerValidation;
            //Declare object which will hold the customer's attributes
            Customer? oCustomer;
          
            //This while loop will run indefinitely until the user press 'x' to quit - Look at the switch case statement
            while (true)
            {
                //Display main menu
                Console.Write("""
                                --- Main Menu ---
                                [1] View a Customer Account
                                [2] Create a Customer Account
                                [3] Remove a Customer Account
                                [*] Log Out
                                [x] Log Out & Quit

                                Enter an option: 
                                """);

                //Prompt user input
                string? sUserInput = InputUtilities.GetInputWithinTimeLimit();

                switch (sUserInput)
                {
                    case "1":
                        //Create a new CustomerValidation object
                        oCustomerValidation = new CustomerValidation();
                        //Retrieve customer's details using the object
                        oCustomer = oCustomerValidation.validateCustomer(clCustomers);
                        //If customer is not null, invoke function
                        if (oCustomer != null)
                        {
                            displayCorrectDetails(oCustomer);
                        }
                        else //Customer is null
                        {
                            //Clear the console
                            Console.Clear();
                            //Display message
                            Console.WriteLine("Customer Not Found\n");
                        }
                        break;
                    case "2":
                        //Clear the console
                        Console.Clear();
                        //Invoke function in the Customer Utilities class to create a new customer account
                        Customer oNewCustomer = CustomerUtilities.CreateCustomer();
                        //Add customer object to the list
                        clCustomers.Add(oNewCustomer);
                        //Invoke function in the Customer Utilities class to add the new customer account to the csv file
                        CustomerUtilities.AddCustomerDetails(oNewCustomer);
                        break;
                    case "3":
                        //Retrieve customer's details by creating a new CustomerValidation object
                        oCustomerValidation = new CustomerValidation();
                        //Retrieve customer's details using the object
                        oCustomer = oCustomerValidation.validateCustomer(clCustomers);
                        //If customer is not null, invoke function
                        if (oCustomer != null)
                        {
                            //Remove the customer object from the list
                            clCustomers.Remove(oCustomer);
                            //Invoke function in the Customer Utilities class to remove the customer account from the csv file
                            CustomerUtilities.RemoveCustomerDetails(oCustomer);
                        }
                        else //Customer is null
                        {
                            //Clear the console
                            Console.Clear();
                            //Display message
                            Console.WriteLine("Customer Not Found\n");
                        }
                        break;
                    case "*":
                        //Log teller out
                        oTeller.logout();
                        //Pause the application for 1 second
                        Thread.Sleep(1000);
                        //Clear the console
                        Console.Clear();
                        //Prompt the teller to log back in
                        oTeller.login();
                        break;
                    case "x":
                        //Logs teller out
                        oTeller.logout();
                        //Pause the application for 1 second
                        Thread.Sleep(1000);
                        //Print message
                        Console.Write("\nExiting...");
                        //Pause the application for 1 second
                        Thread.Sleep(1000);
                        //Quit the application
                        Environment.Exit(0);
                        break;
                    default:
                        //Clear the console
                        Console.Clear();
                        //Print message
                        Console.WriteLine("Invalid Input. Please try again.\n");
                        break;
                }
            }
        }

        /*This method stores the data from the Customers.csv file and stores it directly into the string list. The string list is returned.*/
        private static List<string> populateStringList()
        {
            //Populate customer list with data from the Customers.csv file
            List<string> slCustomers = new List<string>();
            //Initialise local variable that contains the name of the csv file
            string sFileName = "Customers.csv"; //Note the csv file is in bin\debug\net8.0

            try
            {
                //Open the file using a stream reader
                using (StreamReader oReader = File.OpenText(sFileName))
                {
                    //Initialise a string that will store the content from the file
                    string sFileInput = "";
                    //Read the whole file
                    while (!oReader.EndOfStream)
                    {
                        //Store a line into a string
                        sFileInput = oReader.ReadLine();
                        //Append the string to the list
                        slCustomers.Add(sFileInput);
                    }
                }
            }
            //Catch exception if file has not been found
            catch (Exception oException)
            {
                //Print message
                Console.WriteLine($"File '{sFileName}' not found");
            }

            //Return string list
            return slCustomers;
        }
        
        /*This method takes the data from the string list, splits the string into a string array and assigns values to the customer's attributes*/
        private static List<Customer> populateCustomerList(List<string> slCustomers)
        {
            //Populate customer list with data from the string list
            List<Customer> clCustomers = new List<Customer>();

            //Iterate through each record
            foreach (string sCustomerDetails in slCustomers)
            {
                //Split the record string by commas
                string[] saCustomerDetails = sCustomerDetails.Split(',');
                //Store the date of birth in the correct format
                DateOnly doDob = DateOnly.Parse(saCustomerDetails[3]);
                //Store the account creation date in the correct format
                DateOnly doCreationDate = DateOnly.Parse(saCustomerDetails[5]);

                //Initialise local variable that will store the account numbers associated with the customer account
                List<string> slAccountNumbers = new List<string>();

                //If the length is greater than or equal to 9, that means there must be at least 1 account number
                if (saCustomerDetails.Length >= 9)
                {
                    //Iterate through each account number
                    for (int iCount = 8; iCount < saCustomerDetails.Length; iCount++)
                    {
                        //Add each account number to the string list
                        slAccountNumbers.Add(saCustomerDetails[iCount]);
                    }
                }

                //Assign all customer's attributes to the Customer object and add the customer object to the list
                clCustomers.Add(new Customer(saCustomerDetails[0].ToString(),   //First Name
                                             saCustomerDetails[1].ToString(),   //Last Name
                                             saCustomerDetails[2].ToString(),   //Other Name
                                             doDob,                             //Date Of Birth
                                             saCustomerDetails[4].ToString(),   //Postcode
                                             saCustomerDetails[6].ToString(),   //Security Question
                                             saCustomerDetails[7].ToString(),   //Security Answer
                                             doCreationDate,                    //Account Creation Date
                                             slAccountNumbers));                //Account Numbers
            }
            //Return customer list
            return clCustomers;
        }

        /*This method displays the correct customer account details based on the associated account numbers*/
        private static void displayCorrectDetails(Customer oCustomer)
        {
            //If the customer has 1 account number, store the account details into a variable
            if (oCustomer.ListOfAccounts.Count == 1)
            {
                //Store account details
                Account oAccount = AccountUtilities.LoadAccountDetails(oCustomer.ListOfAccounts[0], oCustomer);
                //Display account options based on the account number and customer
                oAccount.AccountOptionsLoop();
                //Clear console
                Console.Clear();
            }
            //Else if the customer has multiple account numbers
            else if (oCustomer.ListOfAccounts.Count > 1)
            {
                string? sAccountNumber;
                //Set boolean to false
                bool bInputValid = false;
                //Store the string
                string sOutput = "The customer has multiple account numbers:\n";
                //Append all account numbers to the output string
                foreach (string sAccountNum in oCustomer.ListOfAccounts) sOutput += $"{sAccountNum}\n";
                do
                {
                    //Display message
                    Console.Write($"{sOutput}\nPlease select one account number: ");
                    //Prompt user to select an account number
                    sAccountNumber = InputUtilities.GetInputWithinTimeLimit();

                    //If the user input is not null and exclusively numeric
                    if (!string.IsNullOrEmpty(sAccountNumber) && checkIfNumeric(sAccountNumber))
                    {
                        //Go through each account number
                        foreach (string sAccountNum in oCustomer.ListOfAccounts)
                        {
                            //Check if user input matches with any of the account numbers, set boolean to true and break out of the foreach loop
                            if (sAccountNumber.CompareTo(sAccountNum) == 0)
                            {
                                //Set boolean to true
                                bInputValid = true;
                                //Break out of the foreach loop
                                break;
                            }
                        }
                        //If the user input is invalid, clear the console and display the message
                        if (!bInputValid)
                        {
                            //Clear console
                            Console.Clear();
                            //Print message
                            Console.WriteLine("Account number has not been chosen. Please try again.\n");
                        }
                    }
                    //If the user input is invalid, clear the console and display the message
                    else
                    {
                        //Clear console
                        Console.Clear();
                        //Print message
                        Console.WriteLine("Invalid Input. Please try again.\n");
                    }
                } while (!bInputValid); //The loop will keep executing until the input is valid
                //Store account details
                Account oAccount = AccountUtilities.LoadAccountDetails(sAccountNumber, oCustomer);
                //Display account options based on the account number and customer
                oAccount.AccountOptionsLoop();
                //Clear console
                Console.Clear();
            }
        }

        public static bool checkIfNumeric(string sInput)
        {
            //Iterate through each charcater and if a non-digit character is found, return false
            foreach (char c in sInput) if (!char.IsDigit(c)) return false;
            //Return true if all characters are unique
            return true;
        }
    }
}